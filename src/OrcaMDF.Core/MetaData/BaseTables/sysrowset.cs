namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysrowset : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("rowsetid", "bigint"),
				new DataColumn("ownertype", "tinyint"),
				new DataColumn("idmajor", "int"),
				new DataColumn("idminor", "int"),
				new DataColumn("numpart", "int"),
				new DataColumn("status", "int"),
				new DataColumn("fgidfs", "smallint"),
				new DataColumn("rcrows", "bigint"),
				new DataColumn("cmprlevel", "tinyint"),
				new DataColumn("fillfact", "tinyint"),
				new DataColumn("maxnullbit", "smallint"),
				new DataColumn("maxleaf", "int"),
				new DataColumn("maxint", "smallint"),
				new DataColumn("minleaf", "smallint"),
				new DataColumn("minint", "smallint"),
				new DataColumn("rsguid", "varbinary", true),
				new DataColumn("lockres", "varbinary", true),
				new DataColumn("dbfragid", "int")
		    });

		public sysrowset() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new sysrowset();
		}

		internal long rowsetid { get { return Field<long>("rowsetid"); } }
		internal byte ownertype { get { return Field<byte>("ownertype"); } }
		internal int idmajor { get { return Field<int>("idmajor"); } }
		internal int idminor { get { return Field<int>("idminor"); } }
		internal int numpart { get { return Field<int>("numpart"); } }
		internal int status { get { return Field<int>("status"); } }
		internal short fgidfs { get { return Field<short>("fgidfs"); } }
		internal long rcrows { get { return Field<long>("rcrows"); } }
		internal byte cmprlevel { get { return Field<byte>("cmprlevel"); } }
		internal byte fillfact { get { return Field<byte>("fillfact"); } }
		internal short maxnullbit { get { return Field<short>("maxnullbit"); } }
		internal int maxleaf { get { return Field<int>("maxleaf"); } }
		internal short maxint { get { return Field<short>("maxint"); } }
		internal short minleaf { get { return Field<short>("minleaf"); } }
		internal short minint { get { return Field<short>("minint"); } }
		internal byte[] rsguid { get { return Field<byte[]>("rsguid"); } }
		internal byte[] lockres { get { return Field<byte[]>("lockres"); } }
		internal int dbfragid { get { return Field<int>("dbfragid"); } }
	}
}