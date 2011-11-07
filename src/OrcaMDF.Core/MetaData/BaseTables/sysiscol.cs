namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysiscol : Row
	{
		public sysiscol()
		{
			Columns.Add(new DataColumn("idmajor", "int"));
			Columns.Add(new DataColumn("idminor", "int"));
			Columns.Add(new DataColumn("subid", "int"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("intprop", "int"));
			Columns.Add(new DataColumn("tinyprop1", "tinyint"));
			Columns.Add(new DataColumn("tinyprop2", "tinyint"));
		}

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