using System;

namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysscalartype : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("id", "int"),
				new DataColumn("schid", "int"),
				new DataColumn("name", "sysname"),
				new DataColumn("xtype", "tinyint"),
				new DataColumn("length", "smallint"),
				new DataColumn("prec", "tinyint"),
				new DataColumn("scale", "tinyint"),
				new DataColumn("collationid", "int"),
				new DataColumn("status", "int"),
				new DataColumn("created", "datetime"),
				new DataColumn("modified", "datetime"),
				new DataColumn("dflt", "int"),
				new DataColumn("chk", "int")
		    });

		public sysscalartype() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new sysscalartype();
		}

		public int id { get { return Field<int>("id"); } }
		public int schid { get { return Field<int>("schid"); } }
		public string name { get { return Field<string>("name"); } }
		public byte xtype { get { return Field<byte>("xtype"); } }
		public short length { get { return Field<short>("length"); } }
		public byte prec { get { return Field<byte>("prec"); } }
		public byte scale { get { return Field<byte>("scale"); } }
		public int collationid { get { return Field<int>("collationid"); } }
		public int status { get { return Field<int>("status"); } }
		public DateTime created { get { return Field<DateTime>("created"); } }
		public DateTime modified { get { return Field<DateTime>("modified"); } }
		public int dflt { get { return Field<int>("dflt"); } }
		public int chk { get { return Field<int>("chk"); } }
	}
}