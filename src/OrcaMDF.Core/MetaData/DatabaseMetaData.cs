using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData.Enumerations;
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

		public DataRow GetEmptyDataRowByTableName(string tableName)
		{
			// Get table
			var sysobject = SysObjects
				.Where(x => x.Name == tableName && (x.Type == "U " || x.Type == "S "))
				.SingleOrDefault();

			if (sysobject == null)
				throw new ArgumentException("Table " + tableName + " does not exist.");

			// Get columns
			var syscols = SysRowsetColumns
				.Where(x => x.ObjectID == sysobject.ObjectID);

			// Create table and add columns
			var dataRow = new DataRow();
			
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

		private void parseSysRowsets()
		{
			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == (long) FixedSystemObjectAllocationUnits.sysrowsets)
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