using System;

namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysschobj : Row
	{
		public sysschobj()
		{
			Columns.Add(new DataColumn("id", "int"));
			Columns.Add(new DataColumn("name", "sysname"));
			Columns.Add(new DataColumn("nsid", "int"));
			Columns.Add(new DataColumn("nsclass", "tinyint"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("type", "char(2)"));
			Columns.Add(new DataColumn("pid", "int"));
			Columns.Add(new DataColumn("pclass", "tinyint"));
			Columns.Add(new DataColumn("intprop", "int"));
			Columns.Add(new DataColumn("created", "datetime"));
			Columns.Add(new DataColumn("modified", "datetime"));
		}

		public override Row NewRow()
		{
			return new sysschobj();
		}

		internal int id { get { return Field<int>("id"); } }
		internal string name { get { return Field<string>("name"); } }
		internal int nsid { get { return Field<int>("nsid"); } }
		internal byte nsclass { get { return Field<byte>("nsclass"); } }
		internal int status { get { return Field<int>("status"); } }
		internal string type { get { return Field<string>("type"); } }
		internal int pid { get { return Field<int>("pid"); } }
		internal byte pclass { get { return Field<byte>("pclass"); } }
		internal int intprop { get { return Field<int>("intprop"); } }
		internal DateTime created { get { return Field<DateTime>("created"); } }
		internal DateTime modified { get { return Field<DateTime>("modified"); } }
	}
}