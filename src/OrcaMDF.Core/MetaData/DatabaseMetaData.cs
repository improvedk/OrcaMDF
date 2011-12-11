using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData.Enumerations;
using OrcaMDF.Core.MetaData.Exceptions;

namespace OrcaMDF.Core.MetaData
{
	public class DatabaseMetaData
	{
		private readonly Database db;

		internal DatabaseMetaData(Database db)
		{
			this.db = db;
		}

		public DataRow GetEmptyIndexRow(string tableName, string indexName)
		{
			// Get table
			var table = db.Dmvs.Objects
				.Where(x => x.Name == tableName && (x.Type == ObjectType.USER_TABLE || x.Type == ObjectType.SYSTEM_TABLE))
				.SingleOrDefault();

			if (table == null)
				throw new UnknownTableException(tableName);

			// Get index
			var index = db.Dmvs.Indexes
				.Where(i => i.ObjectID == table.ObjectID && i.Name == indexName)
				.SingleOrDefault();

			if (index == null)
				throw new UnknownIndexException(tableName, indexName);

			if (index.IndexID == 0)
				throw new ArgumentException("Can't create DataRow for heaps.");

			// Determine if table is clustered or a heap. If we're not scanning the clustered index itself, see if
			// table has a clustered index. If not, it's a heap.
			bool isHeap = true;
			if (index.IndexID == 1 || db.Dmvs.Indexes.Any(i => i.ObjectID == table.ObjectID && i.IndexID == 1))
				isHeap = false;

			// Get index columns
			var idxColumns = db.Dmvs.IndexColumns
				.Join(db.Dmvs.Columns, ic => new { ic.ColumnID, ic.ObjectID }, c => new { c.ColumnID, c.ObjectID }, (ic, c) => new { ic.ObjectID, ic.IndexID, ic.KeyOrdinal, c.IsNullable, ic.IsIncludedColumn, c.SystemTypeID, c.Name, c.MaxLength })
				.Where(x => x.ObjectID == table.ObjectID && x.IndexID == index.IndexID)
				.OrderBy(x => x.KeyOrdinal);

			// Get first index partition
			var idxFirstPartition = db.Dmvs.SystemInternalsPartitions
				.Where(p => p.ObjectID == table.ObjectID && p.IndexID == index.IndexID)
				.OrderBy(p => p.PartitionNumber)
				.First();

			if (idxColumns.Count() == 0)
				throw new Exception("No columns found for index '" + indexName + "'");

			// Get rowset columns - these are the ones implicitly included in the index
			var partitionColumns = db.Dmvs.SystemInternalsPartitionColumns
				.Where(pc => pc.PartitionID == idxFirstPartition.PartitionID && pc.KeyOrdinal > idxColumns.Max(ic => ic.KeyOrdinal))
				.OrderBy(pc => pc.KeyOrdinal);

			// Add columns as specified in sysiscols
			var columnsList = new List<DataColumn>();
			foreach(var col in idxColumns)
			{
				var sqlType = db.Dmvs.Types.Where(x => x.SystemTypeID == col.SystemTypeID).Single();

				// TODO: Handle decimal/other data types that needs more than a length specification

				var dc = new DataColumn(col.Name, sqlType.Name + "(" + col.MaxLength + ")");
				dc.IsNullable = col.IsNullable;
				dc.IsIncluded = col.IsIncludedColumn;

				columnsList.Add(dc);
			}
			
			// Add remaining columns as specified in sysrscols
			foreach (var col in partitionColumns)
			{
				var sqlType = db.Dmvs.Types.Where(x => x.UserTypeID == col.SystemTypeID).Single();

				// The uniquifier for clustered tables needs special treatment. Uniquifier is detected by the system type and
				// the fact that it's stored in the variable length section of the record (LeafOffset < 0).
				if (!isHeap && col.SystemTypeID == (int)SystemType.Int && col.LeafOffset < 0)
				{
					columnsList.Add(DataColumn.Uniquifier);
					continue;
				}

				// The RID for heaps needs special treatment. RID is detected by system type (binary(8)) and by
				// being the last column in the record.
				if (isHeap && col.SystemTypeID == (int)SystemType.Binary && col.MaxLength == 8 && col.KeyOrdinal == partitionColumns.Max(pc => pc.KeyOrdinal))
				{
					columnsList.Add(DataColumn.RID);
					continue;
				}

				// We don't have the corresponding column name from the clustered key (though it could be queried).
				// Thus we'll just give them an internal name for now.
				var dc = new DataColumn("__rscol_" + col.KeyOrdinal, sqlType.Name + "(" + col.MaxLength + ")");
				dc.IsNullable = col.IsNullable;

				// Clustered index columns that are not explicitly included in the nonclustered index will be
				// implicitly included.
				dc.IsIncluded = true;

				columnsList.Add(dc);
			}

			return new DataRow(columnsList);
		}

		/// <summary>
		/// Looks up the partition and determines if it's using vardecimal columns.
		/// 
		/// Decimals are special - they may in fact be vardecimals, though type wise there is no distinction in SQL Server.
		/// Determining whether a given decimal column is vardecimal is done through the built-in OBJECTPROPERTY function,
		/// however, we do not have access to that. There's a workaround though. Decimals are _always_ fixed length...
		/// _Except_ when they're vardecimal. If we look in sys.system_internals_partition_columns, all fixed length
		/// columns have a positive leaf_offset, where as variable length columns have a negative leaf_offset value - 
		/// including vardecimal. As vardecimals are either all decimal or all vardecimal, we just need to determine
		/// if _any_ decimal column, for this table, has a negative leaf_offset value.
		/// </summary>
		internal bool PartitionHasVardecimalColumns(long partitionID)
		{
			// Get the vardecimal type id
			byte vardecimalTypeID = db.Dmvs.Types
				.Where(t => t.Name == "decimal")
				.Select(t => t.SystemTypeID)
				.Single();

			// Get all partition columns of type decimal with a negative leaf_offset
			int negativeLeafOffsetDecimalColumns = db.Dmvs.SystemInternalsPartitionColumns
				.Join(db.Dmvs.SystemInternalsPartitions, pc => pc.PartitionID, p => p.PartitionID, (pc, p) => new { Column = pc, Partition = p })
				.Where(x => x.Partition.PartitionID == partitionID)
				.Where(x => x.Column.SystemTypeID == vardecimalTypeID)
				.Where(x => x.Column.LeafOffset < 0)
				.Count();

			// If any decimal columns are stored as variable length, we're using vardecimals
			return negativeLeafOffsetDecimalColumns > 0;
		}

		public DataRow GetEmptyDataRow(string tableName)
		{
			// Get table
			var table = db.Dmvs.Objects
				.Where(x => x.Name == tableName && (x.Type == ObjectType.USER_TABLE || x.Type == ObjectType.SYSTEM_TABLE))
				.SingleOrDefault();

			if (table == null)
				throw new UnknownTableException(tableName);

			// Get index
			var clusteredIndex = db.Dmvs.Indexes
				.Where(i => i.ObjectID == table.ObjectID && i.IndexID == 1)
				.SingleOrDefault();
			
			// Get columns
			var syscols = db.Dmvs.Columns
				.Where(x => x.ObjectID == table.ObjectID);

			// Create table and add columns
			var columnsList = new List<DataColumn>();

			// If it's a non unique clustered index, add uniquifier column
			if (clusteredIndex != null && !clusteredIndex.IsUnique)
				columnsList.Add(DataColumn.Uniquifier);
			
			foreach(var col in syscols)
			{
				var sqlType = db.Dmvs.Types.Where(x => x.SystemTypeID == col.SystemTypeID && x.UserTypeID == x.SystemTypeID).Single();
				DataColumn dc;

				switch((SystemType)sqlType.SystemTypeID)
				{
					case SystemType.Decimal:
						dc = new DataColumn(col.Name, sqlType.Name + "(" + col.Precision + "," + col.Scale + ")");
						break;

					default:
						dc = new DataColumn(col.Name, sqlType.Name + "(" + col.MaxLength + ")");
						break;
				}

				dc.IsNullable = sqlType.IsNullable;
				dc.IsSparse = col.IsSparse;
				dc.ColumnID = col.ColumnID;

				columnsList.Add(dc);
			}

			return new DataRow(columnsList);
		}
	}
}