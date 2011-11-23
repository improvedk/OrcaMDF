namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class syssingleobjref : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("class", "tinyint"),
				new DataColumn("depid", "int"),
				new DataColumn("depsubid", "int"),
				new DataColumn("indepid", "int"),
				new DataColumn("indepsubid", "int"),
				new DataColumn("status", "int")
		    });

		public syssingleobjref() : base(schema)
		{ }

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