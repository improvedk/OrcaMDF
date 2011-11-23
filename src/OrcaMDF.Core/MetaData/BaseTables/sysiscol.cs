namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysiscol : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("idmajor", "int"),
				new DataColumn("idminor", "int"),
				new DataColumn("subid", "int"),
				new DataColumn("status", "int"),
				new DataColumn("intprop", "int"),
				new DataColumn("tinyprop1", "tinyint"),
				new DataColumn("tinyprop2", "tinyint")
		    });

		public sysiscol() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new sysiscol();
		}

		public int idmajor { get { return Field<int>("idmajor"); } }
		public int idminor { get { return Field<int>("idminor"); } }
		public int subid { get { return Field<int>("subid"); } }
		public int status { get { return Field<int>("status"); } }
		public int intprop { get { return Field<int>("intprop"); } }
		public byte tinyprop1 { get { return Field<byte>("tinyprop1"); } }
		public byte tinyprop2 { get { return Field<byte>("tinyprop2"); } }
	}
}