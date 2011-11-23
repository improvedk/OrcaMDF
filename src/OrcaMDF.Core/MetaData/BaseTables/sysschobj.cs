using System;

namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysschobj : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("id", "int"),
				new DataColumn("name", "sysname"),
				new DataColumn("nsid", "int"),
				new DataColumn("nsclass", "tinyint"),
				new DataColumn("status", "int"),
				new DataColumn("type", "char(2)"),
				new DataColumn("pid", "int"),
				new DataColumn("pclass", "tinyint"),
				new DataColumn("intprop", "int"),
				new DataColumn("created", "datetime"),
				new DataColumn("modified", "datetime")
		    });

		public sysschobj() : base(schema)
		{ }

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