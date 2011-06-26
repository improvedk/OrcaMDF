namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.sysrowsets
	/// Column names inspired by http://www.g-productions.nl/index.php?name=system_internals_partitions&version=2008SP2
	/// </summary>
	public class SysRowset : Row
	{
		public SysRowset()
		{
			Columns.Add(new DataColumn("rowsetid", "bigint"));
			Columns.Add(new DataColumn("ownertype", "tinyint"));
			Columns.Add(new DataColumn("idmajor", "int"));
			Columns.Add(new DataColumn("idminor", "int"));
			Columns.Add(new DataColumn("numpart", "int"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("fgidfs", "smallint"));
			Columns.Add(new DataColumn("rcrows", "bigint"));
			Columns.Add(new DataColumn("cmprlevel", "tinyint"));
			Columns.Add(new DataColumn("fillfact", "tinyint"));
			Columns.Add(new DataColumn("maxnullbit", "smallint"));
			Columns.Add(new DataColumn("maxleaf", "int"));
			Columns.Add(new DataColumn("maxint", "smallint"));
			Columns.Add(new DataColumn("minleaf", "smallint"));
			Columns.Add(new DataColumn("minint", "smallint"));
			Columns.Add(new DataColumn("rsguid", "varbinary", true));
			Columns.Add(new DataColumn("lockres", "varbinary", true));
			Columns.Add(new DataColumn("dbfragid", "int"));
		}

		public override Row NewRow()
		{
			return new SysRowset();
		}

		public long PartitionID { get { return Field<long>("rowsetid"); } }
		public byte OwnerType { get { return Field<byte>("ownertype"); } }
		public int ObjectID { get { return Field<int>("idmajor"); } }
		public int IndexID { get { return Field<int>("idminor"); } }
		public int PartitionNumber { get { return Field<int>("numpart"); } }
		public int Status { get { return Field<int>("status"); } }
		public short FilestreamFilegroupID { get { return Field<short>("fgidfs"); } }
		public long Rows { get { return Field<long>("rcrows"); } }
		public byte CompressionLevel { get { return Field<byte>("cmprlevel"); } }
		public byte FillFactor { get { return Field<byte>("fillfact"); } }
		public short MaxNullBit { get { return Field<short>("maxnullbit"); } }
		public int MaxLeafLength { get { return Field<int>("maxleaf"); } }
		public short MaxInternalLength { get { return Field<short>("maxint"); } }
		public short MinLeafLength { get { return Field<short>("minleaf"); } }
		public short MinInternalLength { get { return Field<short>("minint"); } }
		public byte[] FilestreamGUID { get { return Field<byte[]>("rsguid"); } }
		public byte[] LockRes { get { return Field<byte[]>("lockres"); } }
		public int DBFragID { get { return Field<int>("dbfragid"); } }

		public override string ToString()
		{
			return "{rowsetid: " + PartitionID + ", idmajor: " + ObjectID + ", idminor: " + IndexID + "}";
		}
	}
}