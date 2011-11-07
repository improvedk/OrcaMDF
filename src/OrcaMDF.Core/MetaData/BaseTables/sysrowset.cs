namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysrowset : Row
	{
		public sysrowset()
		{
			Columns.Add(new DataColumn("rowsetid", "bigint"));
			Columns.Add(new DataColumn("ownertype", "tinyint"));
			Columns.Add(new DataColumn("idmajor", "int"));
			Columns.Add(new DataColumn("idminor", "int"));
			Columns.Add(new DataColumn("numpart", "int"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("fgidfs", "smallint"));
			Columns.Add(new DataColumn("rcrows", "bigint"));
			Columns.Add(new DataColumn("cmprlevel", "tinyint"));
			Columns.Add(new DataColumn("fillfact", "tinyint"));
			Columns.Add(new DataColumn("maxnullbit", "smallint"));
			Columns.Add(new DataColumn("maxleaf", "int"));
			Columns.Add(new DataColumn("maxint", "smallint"));
			Columns.Add(new DataColumn("minleaf", "smallint"));
			Columns.Add(new DataColumn("minint", "smallint"));
			Columns.Add(new DataColumn("rsguid", "varbinary", true));
			Columns.Add(new DataColumn("lockres", "varbinary", true));
			Columns.Add(new DataColumn("dbfragid", "int"));
		}

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