namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class syscolpar : Row
	{
		public syscolpar()
		{
			Columns.Add(new DataColumn("id", "int"));
			Columns.Add(new DataColumn("number", "smallint"));
			Columns.Add(new DataColumn("colid", "int"));
			Columns.Add(new DataColumn("name", "sysname", true));
			Columns.Add(new DataColumn("xtype", "tinyint"));
			Columns.Add(new DataColumn("utype", "int"));
			Columns.Add(new DataColumn("length", "smallint"));
			Columns.Add(new DataColumn("prec", "tinyint"));
			Columns.Add(new DataColumn("scale", "tinyint"));
			Columns.Add(new DataColumn("collationid", "int"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("maxinrow", "smallint"));
			Columns.Add(new DataColumn("xmlns", "int"));
			Columns.Add(new DataColumn("dflt", "int"));
			Columns.Add(new DataColumn("chk", "int"));
			Columns.Add(new DataColumn("idtval", "varbinary", true));
		}

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