using System;
using System.Collections.Generic;
using System.Data;
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

		public DataTable GetEmptyDataTableByName(string tableName)
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
			var dt = new DataTable();
			
			foreach(var col in syscols)
			{
				var sqlType = SysScalarTypes.Where(x => x.ID == col.XType).Single();
				var clrType = getManagedTypeFromSqlType(sqlType.Name);

				var dc = new DataColumn(col.Name, clrType);
				dc.AllowDBNull = sqlType.IsNullable;
				dc.Caption = sqlType.Name; // Ugly hack, using caption to store original SQL Server type name, should subclass

				// Only string types should have length set, and only string types have a collation
				if(sqlType.CollationID != 0)
					dc.MaxLength = sqlType.Length;

				dt.Columns.Add(dc);
			}

			return dt;
		}

		private Type getManagedTypeFromSqlType(string sqlType)
		{
			/*
				Unsupported types as there is no direct CLR equivalent:
				- hierarchyid
				- geometry
				- geography
				- timestamp
				- xml
			*/

			switch(sqlType)
			{
				case "image":
				case "varbinary":
				case "binary":
					return typeof (byte[]);
				
				case "text":
				case "ntext":
				case "varchar":
				case "char":
				case "nvarchar":
				case "nchar":
				case "sysname":
					return typeof (string);

				case "uniqueidentifier":
					return typeof (Guid);

				case "date":
				case "datetime2":
				case "smalldatetime":
				case "datetime":
					return typeof (DateTime);
 
				case "time":
					return typeof (TimeSpan);

				case "bigint":
					return typeof (long);

				case "tinyint":
					return typeof (byte);

				case "smallint":
					return typeof (short);

				case "int":
					return typeof (int);

				case "bit":
					return typeof (bool);
				
				case "smallmoney":
				case "money":
				case "numeric":
				case "decimal":
					return typeof (decimal);
					
				case "real":
					return typeof (float);
					
				case "float":
					return typeof (double);

				case "sql_variant":
					return typeof (object);

				case "datetimeoffset":
					return typeof (DateTimeOffset);

				default:
					throw new ArgumentException("Unknown type: " + sqlType);
			}
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

			SysObjects = scanner.ScanLinkedPages<SysObject>(new PagePointer(pageLoc)).ToList();
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

			SysScalarTypes = scanner.ScanLinkedPages<SysScalarType>(new PagePointer(pageLoc)).ToList();
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

			SysRowsetColumns = scanner.ScanLinkedPages<SysRowsetColumn>(new PagePointer(pageLoc)).ToList();
		}

		private void parseSysRowsets()
		{
			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == (long) FixedSystemObjectAllocationUnits.sysrowsets)
				.Single()
				.FirstPage;

			SysRowsets = scanner.ScanLinkedPages<SysRowset>(new PagePointer(pageLoc)).ToList();
		}

		private void parseSysAllocationUnits()
		{
			var bootPage = file.GetBootPage();

			SysAllocationUnits = scanner.ScanLinkedPages<SysAllocationUnit>(bootPage.FirstSysIndexes).ToList();
		}
	}
}