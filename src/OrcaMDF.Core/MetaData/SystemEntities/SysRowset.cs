namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.sysrowsets
	/// Column names inspired by http://www.g-productions.nl/index.php?name=system_internals_partitions&version=2008SP2
	/// </summary>
	public class SysRowset
	{
		[Column("bigint", 1)] // rowsetid
		public long PartitionID { get; internal set; }

		[Column("tinyint", 2)] // ownertype
		public byte OwnerType { get; internal set; }

		[Column("int", 3)] // idmajor
		public int ObjectID { get; internal set; }

		[Column("int", 4)] // idminor
		public int IndexID { get; internal set; }

		[Column("int", 5)] // numpart
		public int PartitionNumber { get; internal set; }

		[Column("int", 6)] // status
		public int Status { get; internal set; }

		[Column("smallint", 7)] // fgidfs
		public short FilestreamFilegroupID { get; internal set; }

		[Column("bigint", 8)] // rcrows
		public long Rows { get; internal set; }

		[Column("tinyint", 9)] // cmprlevel
		public byte CompressionLevel { get; internal set; }

		[Column("tinyint", 10)] // fillfact
		public byte FillFactor { get; internal set; }

		[Column("smallint", 11)] // maxnullbit
		public short MaxNullBit { get; internal set; }

		[Column("int", 12)] // maxleaf
		public int MaxLeafLength { get; internal set; }

		[Column("smallint", 13)] // maxint
		public short MaxInternalLength { get; internal set; }

		[Column("smallint", 14)] // minleaf
		public short MinLeafLength { get; internal set; }

		[Column("smallint", 15)] // minint
		public short MinInternalLength { get; internal set; }

		[Column("varbinary", 16, Nullable = true)] // rsguid
		public byte[] FilestreamGUID { get; internal set; }

		[Column("varbinary", 17, Nullable = true)] // lockres
		public byte[] LockRes { get; internal set; }

		[Column("int", 18)] // dbfragid
		public int DBFragID { get; internal set; }
	}
}