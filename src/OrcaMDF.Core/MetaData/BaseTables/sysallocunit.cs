namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysallocunit : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("auid", "bigint"),
				new DataColumn("type", "tinyint"),
				new DataColumn("ownerid", "bigint"),
				new DataColumn("status", "int"),
				new DataColumn("fgid", "smallint"),
				new DataColumn("pgfirst", "binary(6)"),
				new DataColumn("pgroot", "binary(6)"),
				new DataColumn("pgfirstiam", "binary(6)"),
				new DataColumn("pcused", "bigint"),
				new DataColumn("pcdata", "bigint"),
				new DataColumn("pcreserved", "bigint"),
				new DataColumn("dbfragid", "int")
		    });

		public sysallocunit()
			: base(schema)
		{ }

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