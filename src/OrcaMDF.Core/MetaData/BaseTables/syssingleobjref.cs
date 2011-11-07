namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class syssingleobjref : Row
	{
		public syssingleobjref()
		{
			Columns.Add(new DataColumn("class", "tinyint"));
			Columns.Add(new DataColumn("depid", "int"));
			Columns.Add(new DataColumn("depsubid", "int"));
			Columns.Add(new DataColumn("indepid", "int"));
			Columns.Add(new DataColumn("indepsubid", "int"));
			Columns.Add(new DataColumn("status", "int"));
		}

		public override Row NewRow()
		{
			return new syssingleobjref();
		}

		internal int @class { get { return Field<byte>("class"); } }
		internal int depid { get { return Field<int>("depid"); } }
		internal int depsubid { get { return Field<int>("depsubid"); } }
		internal int indepid { get { return Field<int>("indepid"); } }
		internal int indepsubid { get { return Field<int>("indepsubid"); } }
		internal int status { get { return Field<int>("status"); } }
	}
}