using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OrcaMDF.Core.MetaData;
using OrcaMDF.Core.MetaData.Enumerations;

namespace OrcaMDF.Core.Engine
{
	public class DataScanner : Scanner
	{
		public DataScanner(MdfFile file)
			: base(file)
		{ }

		/// <summary>
		/// Will scan any table - heap or clustered - and return an IEnumerable of typed rows with data & schema
		/// </summary>
		public IEnumerable<TDataRow> ScanTable<TDataRow>(string tableName) where TDataRow : Row, new()
		{
			var schema = new TDataRow();

			return scanTable(tableName, schema).Cast<TDataRow>();
		}

		/// <summary>
		/// Will scan any table - heap or clustered - and return an IEnumerable of generic rows with data & schema
		/// </summary>
		public IEnumerable<Row> ScanTable(string tableName)
		{
			var schema = MetaData.GetEmptyDataRow(tableName);

			return scanTable(tableName, schema);
		}

		internal IEnumerable<TDataRow> ScanLinkedDataPages<TDataRow>(PagePointer loc) where TDataRow : Row, new()
		{
			return ScanLinkedDataPages(loc, new TDataRow()).Cast<TDataRow>();
		}

		/// <summary>
		/// Starts at the data page (loc) and follows the NextPage pointer chain till the end.
		/// </summary>
		internal IEnumerable<Row> ScanLinkedDataPages(PagePointer loc, Row schema)
		{
			while (loc != PagePointer.Zero)
			{
				var page = File.GetDataPage(loc);

				foreach (var dr in page.GetEntities(schema))
					yield return dr;

				loc = page.Header.NextPage;
			}
		}

		private IEnumerable<Row> scanTable(string tableName, Row schema)
		{
			// Get object
			var tableObject = MetaData.SysObjects
				.Where(x => x.Name == tableName)
				.Where(x => x.Type == ObjectType.INTERNAL_TABLE || x.Type == ObjectType.SYSTEM_TABLE || x.Type == ObjectType.USER_TABLE)
				.SingleOrDefault();
			if (tableObject == null)
				throw new ArgumentException("Table does not exist.");

			// Get rowset, prefer clustered index if exists
			var tableRowset = MetaData.SysRowsets
				.Where(x => x.ObjectID == tableObject.ObjectID && x.IndexID <= 1)
				.OrderByDescending(x => x.IndexID)
				.FirstOrDefault();
			if (tableRowset == null)
				throw new ArgumentException("Table has no rowsets.");

			// Get allocation unit for in-row data
			var allocUnit = MetaData.SysAllocationUnits
				.Where(x => x.ContainerID == tableRowset.PartitionID && x.Type == 1)
				.SingleOrDefault();
			if (allocUnit == null)
				throw new ArgumentException("Table has no allocation unit.");
			
			// Either scan heap or linked list of pages, depending on index type
			if (tableRowset.IndexID == ReservedIndexID.Heap)
				return scanHeap(allocUnit.FirstIamPage, schema);
			else
				return ScanLinkedDataPages(allocUnit.FirstPage, schema);
		}

		/// <summary>
		/// Scans a heap beginning from the provided IAM page and onwards.
		/// </summary>
		private IEnumerable<Row> scanHeap(PagePointer loc, Row schema)
		{
			while (loc != PagePointer.Zero)
			{
				var iamPage = File.GetIamPage(loc);

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
						var dataPage = File.GetDataPage(pageLoc);

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
				var dataPage = File.GetDataPage(loc);

				foreach(var dr in dataPage.GetEntities(schema))
					yield return dr;
			}
		}
	}
}