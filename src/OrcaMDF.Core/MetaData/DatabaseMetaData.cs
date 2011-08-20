using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData.Enumerations;
using OrcaMDF.Core.MetaData.Exceptions;
using OrcaMDF.Core.MetaData.SystemEntities;

namespace OrcaMDF.Core.MetaData
{
	public class DatabaseMetaData
	{
		public IList<SysAllocationUnit> SysAllocationUnits { get; internal set; }
		public IList<SysObject> SysObjects { get; internal set; }
		public IList<SysRowset> SysRowsets { get; internal set; }
		public IList<SysColPar> SysColPars { get; internal set; }
		public IList<SysScalarType> SysScalarTypes { get; internal set; }

		private IList<SysIndexStat> sysIndexStats;
		public IList<SysIndexStat> SysIndexStats
		{
			get
			{
				if (sysIndexStats == null)
					parseSysIndexStats();

				return sysIndexStats;
			}
		}

		private IList<SysIsCol> sysIsCols;
		public IList<SysIsCol> SysIsCols
		{
			get
			{
				if (sysIsCols == null)
					parseSysIsCols();

				return sysIsCols;
			}
		}

		private IList<Sysrscol> sysrscols;
		public IList<Sysrscol> Sysrscols
		{
			get
			{
				if (sysrscols == null)
					parseSysrscols();

				return sysrscols;
			}
		}

		private readonly MdfFile file;
		private readonly DataScanner scanner;

		internal DatabaseMetaData(MdfFile file)
		{
			this.file = file;
			scanner = new DataScanner(file);

			parseSysAllocationUnits();
			parseSysRowsets();
			parseSysColPars();
			parseSysObjects();
			parseSysScalarTypes();
		}

		private void parseSysrscols()
		{
			sysrscols = scanner.ScanTable<Sysrscol>("sysrscols").ToList();
		}

		public DataRow GetEmptyIndexRow(string tableName, string indexName)
		{
			// Get table
			var table = SysObjects
				.Where(x => x.Name == tableName && (x.Type == "U " || x.Type == "S "))
				.SingleOrDefault();

			if (table == null)
				throw new UnknownTableException(tableName);

			// Get index
			var index = SysIndexStats
				.Where(x => x.ObjectID == table.ObjectID && x.Name == indexName)
				.SingleOrDefault();

			if (index == null)
				throw new UnknownIndexException(tableName, indexName);

			if (index.IndexID == 0)
				throw new ArgumentException("Can't create DataRow for heaps.");

			// Determine if table is clustered or a heap. If we're not scanning the clustered index itself, see if
			// table has a clustered index. If not, it's a heap.
			bool isHeap = true;
			if (index.IndexID == 1 || SysIndexStats.Any(x => x.ObjectID == table.ObjectID && x.IndexID == 1))
				isHeap = false;

			// Get index columns
			var idxColumns = SysIsCols
				.Join(SysColPars, x => new { x.ColumnID, x.ObjectID }, y => new { y.ColumnID, y.ObjectID }, (x, y) => new { x.ObjectID, x.IndexID, x.KeyOrdinal, y.IsNullable, x.IsIncluded, y.XType, y.Name, y.Length })
				.Where(x => x.ObjectID == table.ObjectID && x.IndexID == index.IndexID)
				.OrderBy(x => x.KeyOrdinal);

			if (idxColumns.Count() == 0)
				throw new Exception("No columns found for index '" + indexName + "'");

			// Get rowset columns
			var rsColumns = Sysrscols
				.Where(x => x.RowsetID == index.RowsetID && x.KeyOrdinal > idxColumns.Max(y => y.KeyOrdinal))
				.OrderBy(x => x.KeyOrdinal);

			// Add columns as specified in sysiscols
			var dataRow = new DataRow();
			foreach(var col in idxColumns)
			{
				var sqlType = SysScalarTypes.Where(x => x.ID == col.XType).Single();

				// TODO: Handle decimal/other data types that needs more than a length specification

				var dc = new DataColumn(col.Name, sqlType.Name + "(" + col.Length + ")");
				dc.IsNullable = col.IsNullable;
				dc.IsIncluded = col.IsIncluded;

				dataRow.Columns.Add(dc);
			}
			
			// Add remaining columns as specified in sysrscols
			foreach(var col in rsColumns)
			{
				var sqlType = SysScalarTypes.Where(x => x.ID == col.SystemTypeID).Single();

				// The uniquifier for clustered tables needs special treatment. Uniquifier is detected by the system type and
				// the fact that it's stored in the variable length section of the record (LeafOffset < 0).
				if (!isHeap && col.SystemTypeID == (int)SystemType.Int && col.LeafOffset < 0)
				{
					dataRow.Columns.Add(DataColumn.Uniquifier);
					continue;
				}

				// The RID for heaps needs special treatment. RID is detected by system type (binary(8)) and by
				// being the last column in the record.
				if(isHeap && col.SystemTypeID == (int)SystemType.Binary && col.MaxLength == 8 && col.KeyOrdinal == rsColumns.Max(x => x.KeyOrdinal))
				{
					dataRow.Columns.Add(DataColumn.RID);
					continue;
				}

				// We don't have the corresponding column name from the clustered key (though it could be queried).
				// Thus we'll just give them an internal name for now.
				var dc = new DataColumn("__rscol_" + col.KeyOrdinal, sqlType.Name + "(" + col.MaxLength + ")");
				dc.IsNullable = col.IsNullable;

				// Clustered index columns that are not explicitly included in the nonclustered index will be
				// implicitly included.
				dc.IsIncluded = true;

				dataRow.Columns.Add(dc);
			}

			return dataRow;
		}

		public DataRow GetEmptyDataRow(string tableName)
		{
			// Get table
			var table = SysObjects
				.Where(x => x.Name == tableName && (x.Type == "U " || x.Type == "S "))
				.SingleOrDefault();

			if (table == null)
				throw new UnknownTableException(tableName);

			// Get index
			var clusteredIndex = SysIndexStats
				.Where(x => x.ObjectID == table.ObjectID && x.IndexID == 1)
				.OrderByDescending(x => x.IndexID)
				.SingleOrDefault();
			
			// Get columns
			var syscols = SysColPars
				.Where(x => x.ObjectID == table.ObjectID);

			// Create table and add columns
			var dataRow = new DataRow();

			// If it's a non unique clustered index, add uniquifier column
			if (clusteredIndex != null && !clusteredIndex.IsUnique)
				dataRow.Columns.Add(DataColumn.Uniquifier);
			
			foreach(var col in syscols)
			{
				var sqlType = SysScalarTypes.Where(x => x.ID == col.XType).Single();
				DataColumn dc;

				switch((SystemType)sqlType.XType)
				{
					case SystemType.Decimal:
						dc = new DataColumn(col.Name, sqlType.Name + "(" + col.Prec + "," + col.Scale + ")");
						break;

					default:
						dc = new DataColumn(col.Name, sqlType.Name + "(" + col.Length + ")");
						break;
				}

				dc.IsNullable = sqlType.IsNullable;
				dc.IsSparse = col.IsSparse;
				dc.ColumnID = col.ColumnID;

				dataRow.Columns.Add(dc);
			}

			return dataRow;
		}

		public string[] TableNames
		{
			get
			{
				return SysObjects
					.Where(x => x.Type == ObjectType.INTERNAL_TABLE || x.Type == ObjectType.SYSTEM_TABLE || x.Type == ObjectType.USER_TABLE)
					.Select(x => x.Name)
					.ToArray();
			}
		}

		public string[] UserTableNames
		{
			get
			{
				return SysObjects
					.Where(x => x.Type == ObjectType.USER_TABLE)
					.Select(x => x.Name)
					.ToArray();
			}
		}

		private void parseSysObjects()
		{
			long rowsetID = SysRowsets
				.Where(x => x.ObjectID == (int)SystemObject.sysschobjs && x.IndexID == 1)
				.Single()
				.PartitionID;

			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == rowsetID && x.Type == 1)
				.Single()
				.FirstPage;

			SysObjects = scanner.ScanLinkedDataPages<SysObject>(pageLoc).ToList();
		}

		private void parseSysScalarTypes()
		{
			long rowsetID = SysRowsets
				.Where(x => x.ObjectID == (int)SystemObject.sysscalartypes && x.IndexID == 1)
				.Single()
				.PartitionID;

			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == rowsetID && x.Type == 1)
				.Single()
				.FirstPage;

			SysScalarTypes = scanner.ScanLinkedDataPages<SysScalarType>(pageLoc).ToList();
		}

		private void parseSysColPars()
		{
			long rowsetID =	SysRowsets
				.Where(x => x.ObjectID == (int)SystemObject.syscolpars && x.IndexID == 1)
				.Single()
				.PartitionID;

			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == rowsetID && x.Type == 1)
				.Single()
				.FirstPage;

			SysColPars = scanner.ScanLinkedDataPages<SysColPar>(pageLoc).ToList();
		}

		private void parseSysIndexStats()
		{
			sysIndexStats = scanner.ScanTable<SysIndexStat>("sysidxstats").ToList();
		}

		private void parseSysIsCols()
		{
			sysIsCols = scanner.ScanTable<SysIsCol>("sysiscols").ToList();
		}

		private void parseSysRowsets()
		{
			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == FixedSystemObjectAllocationUnits.sysrowsets)
				.Single()
				.FirstPage;

			SysRowsets = scanner.ScanLinkedDataPages<SysRowset>(pageLoc).ToList();
		}

		private void parseSysAllocationUnits()
		{
			// Though this has a fixed first-page location at (1:16) we'll read it from the boot page to be sure
			var bootPage = file.GetBootPage();
			SysAllocationUnits = scanner.ScanLinkedDataPages<SysAllocationUnit>(bootPage.FirstSysIndexes).ToList();
		}
	}
}