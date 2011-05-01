namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.syscolpars
	/// </summary>
	public class SysRowsetColumn
	{
		[Column("int", 1)] // id
		public int ObjectID { get; internal set; }

		[Column("smallint", 2)] // number
		public short Number { get; internal set; }

		[Column("int", 3)] // colid
		public int ColumnID { get; internal set; }

		[Column("sysname", 4, Nullable = true)] // name
		public string Name { get; internal set; }

		[Column("tinyint", 5)] // xtype
		public byte XType { get; internal set; }

		[Column("int", 6)] // utype
		public int UType { get; internal set; }

		[Column("smallint", 7)] // length
		public short Length { get; internal set; }

		[Column("tinyint", 8)] // prec
		public byte Prec { get; internal set; }

		[Column("tinyint", 9)]  // scale
		public byte Scale { get; internal set; }
		
		[Column("int", 10)] // collationid
		public int CollationID { get; internal set; }

		[Column("int", 11)] // status
		public int status { get; internal set; }

		[Column("smallint", 12)] // maxinrow
		public short MaxInRow { get; internal set; }

		[Column("int", 13)] // xmlns
		public int XmlNS { get; internal set; }

		[Column("int", 14)] // dflt
		public int DFLT { get; internal set; }

		[Column("int", 15)] // chk
		public int CHK { get; internal set; }

		[Column("varbinary", 16, Nullable = true)] // idtval
		public byte[] IDTval { get; internal set; }
	}
}