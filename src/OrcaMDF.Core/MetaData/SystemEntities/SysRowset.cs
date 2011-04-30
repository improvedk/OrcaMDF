namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.sysrowsets
	/// Column names inspired by http://www.g-productions.nl/index.php?name=system_internals_partitions&version=2008SP2
	/// </summary>
	public class SysRowset
	{
		[Column("bigint")] // rowsetid
		public long RowsetID { get; internal set; }

		[Column("tinyint")] // ownertype
		public byte OwnerType { get; internal set; }

		[Column("int")] // idmajor
		public int ObjectID { get; internal set; }

		[Column("int")] // idminor
		public int IndexID { get; internal set; }

		[Column("int")] // numpart
		public int PartitionNumber { get; internal set; }

		[Column("int")] // status
		public int Status { get; internal set; }

		[Column("smallint")] // fgidfs
		public short FilestreamFilegroupID { get; internal set; }

		[Column("bigint")] // rcrows
		public long Rows { get; internal set; }

		[Column("tinyint")] // cmprlevel
		public byte CompressionLevel { get; internal set; }

		[Column("tinyint")] // fillfact
		public byte FillFactor { get; internal set; }

		[Column("smallint")] // maxnullbit
		public short MaxNullBit { get; internal set; }

		[Column("int")] // maxleaf
		public int MaxLeafLength { get; internal set; }

		[Column("smallint")] // maxint
		public short MaxInternalLength { get; internal set; }

		[Column("smallint")] // minleaf
		public short MinLeafLength { get; internal set; }

		[Column("smallint")] // minint
		public short MinInternalLength { get; internal set; }

		[Column("varbinary", Nullable = true)] // rsguid
		public byte[] FilestreamGUID { get; internal set; }

		[Column("varbinary", Nullable = true)] // lockres
		public byte[] LockRes { get; internal set; }

		[Column("int")] // dbfragid
		public int DBFragID { get; internal set; }
	}
}