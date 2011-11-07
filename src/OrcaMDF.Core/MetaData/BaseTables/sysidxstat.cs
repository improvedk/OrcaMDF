namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysidxstat : Row
	{
		public sysidxstat()
		{
			Columns.Add(new DataColumn("id", "int"));
			Columns.Add(new DataColumn("indid", "int"));
			Columns.Add(new DataColumn("name", "sysname", true));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("intprop", "int"));
			Columns.Add(new DataColumn("fillfact", "tinyint"));
			Columns.Add(new DataColumn("type", "tinyint"));
			Columns.Add(new DataColumn("tinyprop", "tinyint"));
			Columns.Add(new DataColumn("dataspace", "int"));
			Columns.Add(new DataColumn("lobds", "int"));
			Columns.Add(new DataColumn("rowset", "bigint"));
		}

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