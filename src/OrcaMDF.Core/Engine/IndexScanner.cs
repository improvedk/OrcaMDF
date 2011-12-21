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
		public IndexScanner(Database database)
			: base(database)
		{ }

		public IEnumerable<Row> ScanIndex(string tableName, string indexName)
		{
			// Get table
			var table = Database.Dmvs.Objects
				.Where(x => x.Name == tableName && (x.Type == ObjectType.USER_TABLE || x.Type == ObjectType.SYSTEM_TABLE))
				.SingleOrDefault();

			if (table == null)
				throw new UnknownTableException(tableName);

			// Get index
			var index = Database.Dmvs.Indexes
				.Where(i => i.ObjectID == table.ObjectID && i.Name == indexName)
				.SingleOrDefault();

			if (index == null)
				throw new UnknownIndexException(tableName, indexName);

			// Depending on index type, scan accordingly
			switch(index.Type)
			{
				case IndexType.Heap:
				case IndexType.Clustered:
					// For both heaps and clustered tables we delegate the responsibility to a DataScanner
					var scanner = new DataScanner(Database);
					return scanner.ScanTable(tableName);

				case IndexType.Nonclustered:
					// Get the schema for the index
					var schema = MetaData.GetEmptyIndexRow(tableName, indexName);

					// Get rowset for the index
					var tableRowset = Database.Dmvs.SystemInternalsPartitions
						.Where(x => x.ObjectID == table.ObjectID && x.IndexID == index.IndexID)
						.FirstOrDefault();

					if (tableRowset == null)
						throw new Exception("Index has no rowset");

					// Get allocation unit for in-row data
					var allocUnit = Database.Dmvs.SystemInternalsAllocationUnits
						.Where(au => au.ContainerID == tableRowset.PartitionID && au.Type == (byte)AllocationUnitType.IN_ROW_DATA)
						.SingleOrDefault();

					if (allocUnit == null)
						throw new ArgumentException("Table has no allocation unit.");

					// Scan the linked list of nonclustered index pages
					// TODO: Support compressed indexes
					return ScanLinkedNonclusteredIndexPages(allocUnit.FirstPage, schema, CompressionContext.NoCompression);

				default:
					throw new ArgumentException("Unsupported index type '" + index.Type + "'");
			}
		}

		/// <summary>
		/// Starts at the data page (loc) and follows the NextPage pointer chain till the end.
		/// </summary>
		internal IEnumerable<Row> ScanLinkedNonclusteredIndexPages(PagePointer loc, Row schema, CompressionContext compression)
		{
			while (loc != PagePointer.Zero)
			{
				var page = Database.GetNonclusteredIndexPage(loc);

				foreach (var dr in page.GetEntities(schema, compression))
					yield return dr;

				loc = page.Header.NextPage;
			}
		}
	}
}