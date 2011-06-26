using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysAllocationUnit : Row
	{
		public SysAllocationUnit()
		{
			Columns.Add(new DataColumn("auid", "bigint"));
			Columns.Add(new DataColumn("type", "tinyint"));
			Columns.Add(new DataColumn("ownerid", "bigint"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("fgid", "smallint"));
			Columns.Add(new DataColumn("pgfirst", "binary(6)"));
			Columns.Add(new DataColumn("pgroot", "binary(6)"));
			Columns.Add(new DataColumn("pgfirstiam", "binary(6)"));
			Columns.Add(new DataColumn("pcused", "bigint"));
			Columns.Add(new DataColumn("pcdata", "bigint"));
			Columns.Add(new DataColumn("pcreserved", "bigint"));
			Columns.Add(new DataColumn("dbfragid", "int"));
		}

		public override Row NewRow()
		{
			return new SysAllocationUnit();
		}
		
		public long AllocationUnitID { get { return Field<long>("auid"); } }
		public byte Type { get { return Field<byte>("type"); } }
		public long ContainerID { get { return Field<long>("ownerid"); } }
		public int Status { get { return Field<int>("status"); } }
		public short FilegroupID { get { return Field<short>("fgid"); } }
		public PagePointer FirstPage { get { return new PagePointer(Field<byte[]>("pgfirst")); } }
		public PagePointer RootPage { get { return new PagePointer(Field<byte[]>("pgroot")); } }
		public PagePointer FirstIamPage { get { return new PagePointer(Field<byte[]>("pgfirstiam")); } }
		public long PCUsed { get { return Field<long>("pcused"); } }
		public long PCData { get { return Field<long>("pcdata"); } }
		public long PCReserved { get { return Field<long>("pcreserved"); } }
		public int DBFragID { get { return Field<int>("dbfragid"); } }
	}
}