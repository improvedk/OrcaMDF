using System.Collections.Generic;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData.DMVs;

namespace OrcaMDF.Core.MetaData
{
	public class DmvGenerator
	{
		private readonly Database db;

		internal DmvGenerator(Database db)
		{
			this.db = db;
		}

		public IEnumerable<Column> Columns
		{
			get { return Column.GetDmvData(db); }
		}

		public IEnumerable<ForeignKey> ForeignKeys
		{
			get { return ForeignKey.GetDmvData(db); }
		}

		public IEnumerable<Index> Indexes
		{
			get { return Index.GetDmvData(db); }
		}

		public IEnumerable<IndexColumn> IndexColumns
		{
			get { return IndexColumn.GetDmvData(db); }
		}

		public IEnumerable<Object> Objects
		{
			get { return Object.GetDmvData(db); }
		}

		public IEnumerable<ObjectDollar> ObjectsDollar
		{
			get { return ObjectDollar.GetDmvData(db); }
		}

		public IEnumerable<Partition> Partitions
		{
			get { return Partition.GetDmvData(db); }
		}

		public IEnumerable<Type> Types
		{
			get { return Type.GetDmvData(db); }
		}

		public IEnumerable<Procedure> Procedures
		{
			get { return Procedure.GetDmvData(db); }
		}

		public IEnumerable<View> Views
		{
			get { return View.GetDmvData(db); }
		}

		public IEnumerable<SqlModule> SqlModules
		{
			get { return SqlModule.GetDmvData(db); }
		}

		public IEnumerable<DatabasePrincipal> DatabasePrincipals
		{
			get { return DatabasePrincipal.GetDmvData(db); }
		}

		public IEnumerable<SystemInternalsAllocationUnit> SystemInternalsAllocationUnits
		{
			get { return SystemInternalsAllocationUnit.GetDmvData(db); }
		}

		public IEnumerable<SystemInternalsPartition> SystemInternalsPartitions
		{
			get { return SystemInternalsPartition.GetDmvData(db); }
		}

		public IEnumerable<SystemInternalsPartitionColumn> SystemInternalsPartitionColumns
		{
			get { return SystemInternalsPartitionColumn.GetDmvData(db); }
		}

		public IEnumerable<Table> Tables
		{
			get { return Table.GetDmvData(db); }
		}
	}
}