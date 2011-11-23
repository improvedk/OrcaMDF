namespace OrcaMDF.Core.MetaData.BaseTables
{
	public class sysrscol : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("rsid", "bigint"),
				new DataColumn("rscolid", "int"),
				new DataColumn("hbcolid", "int"),
				new DataColumn("rcmodified", "bigint"),
				new DataColumn("ti", "int"),
				new DataColumn("cid", "int"),
				new DataColumn("ordkey", "smallint"),
				new DataColumn("maxinrowlen", "smallint"),
				new DataColumn("status", "int"),
				new DataColumn("offset", "int"),
				new DataColumn("nullbit", "int"),
				new DataColumn("bitpos", "smallint"),
				new DataColumn("colguid", "varbinary(16)", true),
				new DataColumn("dbfragid", "int")
		    });

		public sysrscol() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new sysrscol();
		}

		public long rsid { get { return Field<long>("rsid"); } }
		public int rscolid { get { return Field<int>("rscolid"); } }
		public int hbcolid { get { return Field<int>("hbcolid"); } }
		public long rcmodified { get { return Field<long>("rcmodified"); } }
		public int ti { get { return Field<int>("ti"); } }
		public int cid { get { return Field<int>("cid"); } }
		public short ordkey { get { return Field<short>("ordkey"); } }
		public short maxinrowlen { get { return Field<short>("maxinrowlen"); } }
		public int status { get { return Field<int>("status"); } }
		public int offset { get { return Field<int>("offset"); } }
		public int nullbit { get { return Field<int>("nullbit"); } }
		public short bitpos { get { return Field<short>("bitpos"); } }
		public byte[] colguid { get { return Field<byte[]>("colguid"); } }
		public int dbfragid { get { return Field<int>("dbfragid"); } }
	}
}