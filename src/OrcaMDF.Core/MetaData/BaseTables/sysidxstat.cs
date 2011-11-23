namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysidxstat : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("id", "int"),
				new DataColumn("indid", "int"),
				new DataColumn("name", "sysname", true),
				new DataColumn("status", "int"),
				new DataColumn("intprop", "int"),
				new DataColumn("fillfact", "tinyint"),
				new DataColumn("type", "tinyint"),
				new DataColumn("tinyprop", "tinyint"),
				new DataColumn("dataspace", "int"),
				new DataColumn("lobds", "int"),
				new DataColumn("rowset", "bigint")
		    });

		public sysidxstat() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new sysidxstat();
		}

		internal int id { get { return Field<int>("id"); } }
		internal int indid { get { return Field<int>("indid"); } }
		internal string name { get { return Field<string>("name"); } }
		internal int status { get { return Field<int>("status"); } }
		internal int intprop { get { return Field<int>("intprop"); } }
		internal byte fillfact { get { return Field<byte>("fillfact"); } }
		internal byte type { get { return Field<byte>("type"); } }
		internal byte tinyprop { get { return Field<byte>("tinyprop"); } }
		internal int dataspace { get { return Field<int>("dataspace"); } }
		internal int lobds { get { return Field<int>("lobds"); } }
		internal long rowset { get { return Field<long>("rowset"); } }
	}
}