using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.MetaData;
using OrcaMDF.Core.MetaData.Enumerations;

namespace OrcaMDF.Core.Engine
{
	public class DataScanner
	{
		private readonly MdfFile file;

		public DataScanner(MdfFile file)
		{
			this.file = file;
		}

		public IEnumerable<TDataRow> ScanTable<TDataRow>(string tableName) where TDataRow : Row, new()
		{
			var schema = new TDataRow();

			return scanTable(tableName, schema).Cast<TDataRow>();
		}

		public IEnumerable<Row> ScanTable(string tableName)
		{
			var metaData = file.GetMetaData();

			var schema = metaData.GetEmptyDataRowByTableName(tableName);

			return scanTable(tableName, schema);
		}

		private IEnumerable<Row> scanTable(string tableName, Row schema)
		{
			var metaData = file.GetMetaData();

			// Get object
			var tableObject = metaData.SysObjects
				.Where(x => x.Name == tableName)
				.Where(x => x.Type == ObjectType.INTERNAL_TABLE || x.Type == ObjectType.SYSTEM_TABLE || x.Type == ObjectType.USER_TABLE)
				.SingleOrDefault();
			if (tableObject == null)
				throw new ArgumentException("Table does not exist.");

			// Get rowset, prefer clustered index if exists
			var tableRowset = metaData.SysRowsets
				.Where(x => x.ObjectID == tableObject.ObjectID && x.IndexID <= 1)
				.OrderByDescending(x => x.IndexID)
				.FirstOrDefault();
			if (tableRowset == null)
				throw new ArgumentException("Table has no rowsets.");

			// Get allocation unit for in-row data
			var allocUnit = metaData.SysAllocationUnits
				.Where(x => x.ContainerID == tableRowset.PartitionID && x.Type == 1)
				.SingleOrDefault();
			if (allocUnit == null)
				throw new ArgumentException("Table has no allocation unit.");
			
			// Either scan heap or linked list of pages, depending on index type
			if (tableRowset.IndexID == ReservedIndexID.Heap)
				return ScanHeap(allocUnit.FirstIamPage, schema);
			else
				return ScanLinkedDataPages(allocUnit.FirstPage, schema);
		}

		/// <summary>
		/// Scans a heap beginning from the provided IAM page and onwards.
		/// </summary>
		public IEnumerable<Row> ScanHeap(PagePointer loc, Row schema)
		{
			while (loc != PagePointer.Zero)
			{
				var iamPage = file.GetIamPage(loc);

				// Gather results from header slots and yield them afterwards
				var iamPageSlots = new []
				               	{
				               		iamPage.Slot0,
									iamPage.Slot1,
									iamPage.Slot2,
									iamPage.Slot3,
									iamPage.Slot4,
									iamPage.Slot5,
									iamPage.Slot6,
									iamPage.Slot7
				               	};

				foreach (var slot in iamPageSlots)
					foreach (var dr in scanIamSlotPage(slot, schema))
						yield return dr;

				// Then loop through extents and yield results
				foreach (var extent in iamPage.GetAllocatedExtents())
					foreach (var pageLoc in extent.GetPagePointers())
					{
						var dataPage = file.GetDataPage(pageLoc);

						foreach (var dr in dataPage.GetEntities(schema))
							yield return dr;
					}

				loc = iamPage.Header.NextPage;
			}
		}

		private IEnumerable<Row> scanIamSlotPage(PagePointer loc, Row schema)
		{
			if (loc != PagePointer.Zero)
			{
				var dataPage = file.GetDataPage(loc);

				foreach(var dr in dataPage.GetEntities(schema))
					yield return dr;
			}
		}
		
		/// <summary>
		/// Starts at the clustered index page (loc) and follows the b-tree structure till the first datapage, from where it'll do a linked page scan.
		/// </summary>
		public IEnumerable<Row> ScanClusteredIndex<TIndexRow>(PagePointer loc, Row dataSchema) where TIndexRow : ClusteredTableIndexRow, new()
		{
			var page = file.GetClusteredIndexPage(loc);
			var indexRecord = page.GetEntities<TIndexRow>().FirstOrDefault();
			
			if(indexRecord != null)
			{
				if(page.Header.Level > 1)
				{
					// Index level > 1 means next level is index as well. Follow chain.
					foreach(var entity in ScanClusteredIndex<TIndexRow>(indexRecord.PagePointer, dataSchema))
						yield return entity;
				}
				else
				{
					// Index level == 1 means next level is leaf - scan linked data pages.
					foreach(var entity in ScanLinkedDataPages(indexRecord.PagePointer, dataSchema))
						yield return entity;
				}
			}
		}

		public IEnumerable<TDataRow> ScanLinkedDataPages<TDataRow>(PagePointer loc) where TDataRow : Row, new()
		{
			return ScanLinkedDataPages(loc, new TDataRow()).Cast<TDataRow>();
		}

		/// <summary>
		/// Starts at the data page (loc) and follows the NextPage pointer chain till the end.
		/// </summary>
		public IEnumerable<Row> ScanLinkedDataPages(PagePointer loc, Row schema)
		{
			while (loc != PagePointer.Zero)
			{
				var page = file.GetDataPage(loc);

				foreach (var dr in page.GetEntities(schema))
					yield return dr;
				
				loc = page.Header.NextPage;
			}
		}
	}
}