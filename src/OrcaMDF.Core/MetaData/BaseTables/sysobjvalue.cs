namespace OrcaMDF.Core.MetaData.BaseTables
{
	internal class sysobjvalue : Row
	{
		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("valclass", "tinyint"),
				new DataColumn("objid", "int"),
				new DataColumn("subobjid", "int"),
				new DataColumn("valnum", "int"),
				new DataColumn("value", "sql_variant", true),
				new DataColumn("imageval", "varbinary", true)
		    });

		public sysobjvalue()
			: base(schema)
		{ }

		public override Row NewRow()
		{
			return new sysobjvalue();
		}

		public byte valclass { get { return Field<byte>("valclass"); } }
		public int objid { get { return Field<int>("objid"); } }
		public int subobjid { get { return Field<int>("subobjid"); } }
		public int valnum { get { return Field<int>("valnum"); } }
		public byte[] value { get { return Field<byte[]>("value"); } }
		public byte[] imageval { get { return Field<byte[]>("imageval"); } }
	}
}