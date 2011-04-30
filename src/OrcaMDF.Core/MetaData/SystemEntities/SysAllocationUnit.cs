namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.sysallocunits
	/// </summary>
	public class SysAllocationUnit
	{
		[Column("bigint")] // auid
		public long AllocationUnitID { get; internal set; }

		[Column("tinyint")] // type
		public byte Type { get; internal set; }

		[Column("bigint")] // ownerid
		public long OwnerID { get; internal set; }

		[Column("int")] // status
		public int Status { get; internal set; }

		[Column("smallint")] // fgid
		public short FilegroupID { get; internal set; }

		[Column("binary(6)")] // pgfirst
		public byte[] FirstPage { get; internal set; }

		[Column("binary(6)")] // pgroot
		public byte[] RootPage { get; internal set; }

		[Column("binary(6)")] // pgfirstiam
		public byte[] FirstIamPage { get; internal set; }

		[Column("bigint")] // pcused
		public long PCUsed { get; internal set; }

		[Column("bigint")] // pcdata
		public long PCData { get; internal set; }

		[Column("bigint")] // pcreserved
		public long PCReserved { get; internal set; }

		[Column("int")] // dbfragid
		public int DBFragID { get; internal set; }
	}
}