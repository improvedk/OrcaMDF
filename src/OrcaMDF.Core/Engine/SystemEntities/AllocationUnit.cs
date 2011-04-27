namespace OrcaMDF.Core.Engine.SystemEntities
{
	public class AllocationUnit
	{
		[Column("bigint")]
		public long AllocationUnitID { get; set; }

		[Column("tinyint")]
		public byte Type { get; set; }

		[Column("bigint")]
		public long OwnerID { get; set; }

		[Column("int")]
		public int Status { get; set; }

		[Column("smallint")]
		public short FilegroupID { get; set; }

		[Column("binary(6)")]
		public byte[] FirstPage { get; set; }

		[Column("binary(6)")]
		public byte[] RootPage { get; set; }

		[Column("binary(6)")]
		public byte[] FirstIamPage { get; set; }

		[Column("bigint")]
		public long PCUsed { get; set; }

		[Column("bigint")]
		public long PCData { get; set; }

		[Column("bigint")]
		public long PCReserved { get; set; }

		[Column("int")]
		public int DBFragID { get; set; }
	}
}