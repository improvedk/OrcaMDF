namespace OrcaMDF.Core.MetaData.BaseTables
{
	public class sysrscol : Row
	{
		public sysrscol()
		{
			Columns.Add(new DataColumn("rsid", "bigint"));
			Columns.Add(new DataColumn("rscolid", "int"));
			Columns.Add(new DataColumn("hbcolid", "int"));
			Columns.Add(new DataColumn("rcmodified", "bigint"));
			Columns.Add(new DataColumn("ti", "int"));
			Columns.Add(new DataColumn("cid", "int"));
			Columns.Add(new DataColumn("ordkey", "smallint"));
			Columns.Add(new DataColumn("maxinrowlen", "smallint"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("offset", "int"));
			Columns.Add(new DataColumn("nullbit", "int"));
			Columns.Add(new DataColumn("bitpos", "smallint"));
			Columns.Add(new DataColumn("colguid", "varbinary(16)", true));
			Columns.Add(new DataColumn("dbfragid", "int"));
		}

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