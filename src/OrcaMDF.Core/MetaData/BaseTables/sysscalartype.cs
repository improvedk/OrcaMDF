using System;

namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysscalartype : Row
	{
		public sysscalartype()
		{
			Columns.Add(new DataColumn("id", "int"));
			Columns.Add(new DataColumn("schid", "int"));
			Columns.Add(new DataColumn("name", "sysname"));
			Columns.Add(new DataColumn("xtype", "tinyint"));
			Columns.Add(new DataColumn("length", "smallint"));
			Columns.Add(new DataColumn("prec", "tinyint"));
			Columns.Add(new DataColumn("scale", "tinyint"));
			Columns.Add(new DataColumn("collationid", "int"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("created", "datetime"));
			Columns.Add(new DataColumn("modified", "datetime"));
			Columns.Add(new DataColumn("dflt", "int"));
			Columns.Add(new DataColumn("chk", "int"));
		}

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