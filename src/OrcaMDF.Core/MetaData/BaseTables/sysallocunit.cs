namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysallocunit : Row
	{
		public sysallocunit()
		{
			Columns.Add(new DataColumn("auid", "bigint"));
			Columns.Add(new DataColumn("type", "tinyint"));
			Columns.Add(new DataColumn("ownerid", "bigint"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("fgid", "smallint"));
			Columns.Add(new DataColumn("pgfirst", "binary(6)"));
			Columns.Add(new DataColumn("pgroot", "binary(6)"));
			Columns.Add(new DataColumn("pgfirstiam", "binary(6)"));
			Columns.Add(new DataColumn("pcused", "bigint"));
			Columns.Add(new DataColumn("pcdata", "bigint"));
			Columns.Add(new DataColumn("pcreserved", "bigint"));
			Columns.Add(new DataColumn("dbfragid", "int"));
		}

		public override Row NewRow()
		{
			return new sysallocunit();
		}

		internal long auid { get { return Field<long>("auid"); } }
		internal byte type { get { return Field<byte>("type"); } }
		internal long ownerid { get { return Field<long>("ownerid"); } }
		internal int status { get { return Field<int>("status"); } }
		internal short fgid { get { return Field<short>("fgid"); } }
		internal byte[] pgfirst { get { return Field<byte[]>("pgfirst"); } }
		internal byte[] pgroot { get { return Field<byte[]>("pgroot"); } }
		internal byte[] pgfirstiam { get { return Field<byte[]>("pgfirstiam"); } }
		internal long pcused { get { return Field<long>("pcused"); } }
		internal long pcdata { get { return Field<long>("pcdata"); } }
		internal long pcreserved { get { return Field<long>("pcreserved"); } }
		internal int dbfragid { get { return Field<int>("dbfragid"); } }
	}
}