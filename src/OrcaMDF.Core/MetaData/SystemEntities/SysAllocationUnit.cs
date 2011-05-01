namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.sysallocunits
	/// </summary>
	public class SysAllocationUnit
	{
		[Column("bigint", 1)] // auid
		public long AllocationUnitID { get; internal set; }

		[Column("tinyint", 2)] // type
		public byte Type { get; internal set; }

		[Column("bigint", 3)] // ownerid
		public long OwnerID { get; internal set; }

		[Column("int", 4)] // status
		public int Status { get; internal set; }

		[Column("smallint", 5)] // fgid
		public short FilegroupID { get; internal set; }

		[Column("binary(6)", 6)] // pgfirst
		public byte[] FirstPage { get; internal set; }

		[Column("binary(6)", 7)] // pgroot
		public byte[] RootPage { get; internal set; }

		[Column("binary(6)", 8)] // pgfirstiam
		public byte[] FirstIamPage { get; internal set; }

		[Column("bigint", 9)] // pcused
		public long PCUsed { get; internal set; }

		[Column("bigint", 10)] // pcdata
		public long PCData { get; internal set; }

		[Column("bigint", 11)] // pcreserved
		public long PCReserved { get; internal set; }

		[Column("int", 12)] // dbfragid
		public int DBFragID { get; internal set; }
	}
}