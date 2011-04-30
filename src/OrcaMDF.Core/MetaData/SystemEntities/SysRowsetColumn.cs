namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.syscolpars
	/// </summary>
	public class SysRowsetColumn
	{
		[Column("int")] // id
		public int ObjectID { get; internal set; }

		[Column("smallint")] // number
		public short Number { get; internal set; }

		[Column("int")] // colid
		public int ColumnID { get; internal set; }

		[Column("sysname", Nullable = true)] // name
		public string Name { get; internal set; }

		[Column("tinyint")] // xtype
		public byte XType { get; internal set; }

		[Column("int")] // utype
		public int UType { get; internal set; }

		[Column("smallint")] // length
		public short Length { get; internal set; }

		[Column("tinyint")] // prec
		public byte Prec { get; internal set; }

		[Column("tinyint")]  // scale
		public byte Scale { get; internal set; }
		
		[Column("int")] // collationid
		public int CollationID { get; internal set; }

		[Column("int")] // status
		public int status { get; internal set; }

		[Column("smallint")] // maxinrow
		public short MaxInRow { get; internal set; }

		[Column("int")] // xmlns
		public int XmlNS { get; internal set; }

		[Column("int")] // dflt
		public int DFLT { get; internal set; }

		[Column("int")] // chk
		public int CHK { get; internal set; }

		[Column("varbinary", Nullable = true)] // idtval
		public byte[] IDTval { get; internal set; }
	}
}