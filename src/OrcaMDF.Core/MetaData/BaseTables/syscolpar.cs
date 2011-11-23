namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class syscolpar : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("id", "int"),
				new DataColumn("number", "smallint"),
				new DataColumn("colid", "int"),
				new DataColumn("name", "sysname", true),
				new DataColumn("xtype", "tinyint"),
				new DataColumn("utype", "int"),
				new DataColumn("length", "smallint"),
				new DataColumn("prec", "tinyint"),
				new DataColumn("scale", "tinyint"),
				new DataColumn("collationid", "int"),
				new DataColumn("status", "int"),
				new DataColumn("maxinrow", "smallint"),
				new DataColumn("xmlns", "int"),
				new DataColumn("dflt", "int"),
				new DataColumn("chk", "int"),
				new DataColumn("idtval", "varbinary", true)
		    });

		public syscolpar() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new syscolpar();
		}

		internal int id { get { return Field<int>("id"); } }
		internal short number { get { return Field<short>("number"); } }
		internal int colid { get { return Field<int>("colid"); } }
		internal string name { get { return Field<string>("name"); } }
		internal byte xtype { get { return Field<byte>("xtype"); } }
		internal int utype { get { return Field<int>("utype"); } }
		internal short length { get { return Field<short>("length"); } }
		internal byte prec { get { return Field<byte>("prec"); } }
		internal byte scale { get { return Field<byte>("scale"); } }
		internal int collationid { get { return Field<int>("collationid"); } }
		internal int status { get { return Field<int>("status"); } }
		internal short maxinrow { get { return Field<short>("maxinrow"); } }
		internal int xmlns { get { return Field<int>("xmlns"); } }
		internal int dflt { get { return Field<int>("dflt"); } }
		internal int chk { get { return Field<int>("chk"); } }
		internal byte[] idtval { get { return Field<byte[]>("idtval"); } }
	}
}