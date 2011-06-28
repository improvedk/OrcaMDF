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
		public IList<SysRowsetColumn> SysRowsetColumns { get; internal set; }
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

		private readonly MdfFile file;
		private readonly DataScanner scanner;

		internal DatabaseMetaData(MdfFile file)
		{
			this.file = file;
			scanner = new DataScanner(file);

			parseSysAllocationUnits();
			parseSysRowsets();
			parseSysRowsetColumns();
			parseSysObjects();
			parseSysScalarTypes();
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

			// Get index columns
			var columns = SysIsCols
				.Join(SysRowsetColumns, x => new { x.ColumnID, x.ObjectID }, y => new { y.ColumnID, y.ObjectID }, (x, y) => new { x.ObjectID, x.IndexID, x.KeyOrdinal, Included = x.IsIncluded, y.XType, y.Name, y.Length })
				.Where(x => x.ObjectID == table.ObjectID && x.IndexID == index.IndexID)
				.OrderBy(x => x.KeyOrdinal);

			if (columns.Count() == 0)
				throw new Exception("No columns found for index '" + indexName + "'");

			// Depending on index type, add required pointer
			var dataRow = new DataRow();

			// TODO: If table is heap and index is nonclustered - add RID pointer to dataRow

			// Add columns as specified in sysiscols
			foreach(var col in columns)
			{
				var sqlType = SysScalarTypes.Where(x => x.ID == col.XType).Single();

				var dc = new DataColumn(col.Name, sqlType.Name + "(" + col.Length + ")");
				dc.IsNullable = sqlType.IsNullable;
				dc.IsIncluded = col.Included;

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
			var syscols = SysRowsetColumns
				.Where(x => x.ObjectID == table.ObjectID);

			// Create table and add columns
			var dataRow = new DataRow();

			// If it's a non unique clustered index, add uniquifier column
			if (clusteredIndex != null && !clusteredIndex.IsUnique)
				dataRow.Columns.Add(DataColumn.Uniquifier);

			foreach(var col in syscols)
			{
				var sqlType = SysScalarTypes.Where(x => x.ID == col.XType).Single();

				var dc = new DataColumn(col.Name, sqlType.Name + "(" + col.Length + ")");
				dc.IsNullable = sqlType.IsNullable;

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

		private void parseSysRowsetColumns()
		{
			long rowsetID =	SysRowsets
				.Where(x => x.ObjectID == (int)SystemObject.syscolpars && x.IndexID == 1)
				.Single()
				.PartitionID;

			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == rowsetID && x.Type == 1)
				.Single()
				.FirstPage;

			SysRowsetColumns = scanner.ScanLinkedDataPages<SysRowsetColumn>(pageLoc).ToList();
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