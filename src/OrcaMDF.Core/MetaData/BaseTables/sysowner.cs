using System;

namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysowner : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("id", "int"),
				new DataColumn("name", "sysname"),
				new DataColumn("type", "char(1)"),
				new DataColumn("sid", "varbinary", true),
				new DataColumn("password", "varbinary", true),
				new DataColumn("dfltsch", "sysname", true),
				new DataColumn("status", "int"),
				new DataColumn("created", "datetime"),
				new DataColumn("modified", "datetime")
		    });

		public sysowner()
			: base(schema)
		{ }

		public override Row NewRow()
		{
			return new sysowner();
		}

		public int id { get { return Field<int>("id"); } }
		public string name { get { return Field<string>("name"); } }
		public string type { get { return Field<string>("type"); } }
		public byte[] sid { get { return Field<byte[]>("sid"); } }
		public byte[] password { get { return Field<byte[]>("password"); } }
		public string dfltsch { get { return Field<string>("dfltsch"); } }
		public int status { get { return Field<int>("status"); } }
		public DateTime created { get { return Field<DateTime>("created"); } }
		public DateTime modified { get { return Field<DateTime>("modified"); } }
	}
}