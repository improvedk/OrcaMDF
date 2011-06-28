using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.MetaData;
using OrcaMDF.Core.MetaData.Enumerations;
using OrcaMDF.Core.MetaData.Exceptions;

namespace OrcaMDF.Core.Engine
{
	public class IndexScanner : Scanner
	{
		public IndexScanner(MdfFile file)
			: base(file)
		{ }

		public IEnumerable<Row> ScanClusteredTableIndex(string tableName, string indexName)
		{
			// Get table
			var table = MetaData.SysObjects
				.Where(x => x.Name == tableName && x.Type == "U ")
				.SingleOrDefault();

			if (table == null)
				throw new UnknownTableException(tableName);

			// Get index
			var index = MetaData.SysIndexStats
				.Where(x => x.ObjectID == table.ObjectID && x.Name == indexName)
				.SingleOrDefault();

			if (index == null)
				throw new UnknownIndexException(tableName, indexName);

			// Depending on index type, scan accordingly
			switch(index.Type)
			{
				case IndexType.Heap:
				case IndexType.Clustered:
					// For both heaps and clustered tables we delegate the responsibility to a DataScanner
					var scanner = new DataScanner(File);
					return scanner.ScanTable(tableName);

				case IndexType.Nonclustered:
					// Get the schema for the index
					var schema = MetaData.GetEmptyIndexRow(tableName, indexName);

					// Get rowset for the index
					var tableRowset = MetaData.SysRowsets
						.Where(x => x.ObjectID == table.ObjectID && x.IndexID == index.IndexID)
						.FirstOrDefault();

					if (tableRowset == null)
						throw new Exception("Index has no rowset");

					// Get allocation unit for in-row data
					var allocUnit = MetaData.SysAllocationUnits
						.Where(x => x.ContainerID == tableRowset.PartitionID && x.Type == 1)
						.SingleOrDefault();

					if (allocUnit == null)
						throw new ArgumentException("Table has no allocation unit.");

					// Scan the linked list of nonclustered index pages
					return ScanLinkedNonclusteredIndexPages(allocUnit.FirstPage, schema);

				default:
					throw new ArgumentException("Unsupported index type '" + index.Type + "'");
			}
		}

		/// <summary>
		/// Starts at the data page (loc) and follows the NextPage pointer chain till the end.
		/// </summary>
		internal IEnumerable<Row> ScanLinkedNonclusteredIndexPages(PagePointer loc, Row schema)
		{
			while (loc != PagePointer.Zero)
			{
				var page = File.GetNonclusteredIndexPage(loc);

				foreach (var dr in page.GetEntities(schema))
					yield return dr;

				loc = page.Header.NextPage;
			}
		}
	}
}