namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class AllocationUnit
	{
		[Column("bigint")]
		public long AllocationUnitID { get; internal set; }

		[Column("tinyint")]
		public byte Type { get; internal set; }

		[Column("bigint")]
		public long OwnerID { get; internal set; }

		[Column("int")]
		public int Status { get; internal set; }

		[Column("smallint")]
		public short FilegroupID { get; internal set; }

		[Column("binary(6)")]
		public byte[] FirstPage { get; internal set; }

		[Column("binary(6)")]
		public byte[] RootPage { get; internal set; }

		[Column("binary(6)")]
		public byte[] FirstIamPage { get; internal set; }

		[Column("bigint")]
		public long PCUsed { get; internal set; }

		[Column("bigint")]
		public long PCData { get; internal set; }

		[Column("bigint")]
		public long PCReserved { get; internal set; }

		[Column("int")]
		public int DBFragID { get; internal set; }
	}
}