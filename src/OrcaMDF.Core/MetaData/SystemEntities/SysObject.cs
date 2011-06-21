using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysObject : DataRow
	{
		public SysObject()
		{
			Columns.Add(new DataColumn("id", "int"));
			Columns.Add(new DataColumn("name", "sysname"));
			Columns.Add(new DataColumn("nsid", "int"));
			Columns.Add(new DataColumn("nsclass", "tinyint"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("type", "char(2)"));
			Columns.Add(new DataColumn("pid", "int"));
			Columns.Add(new DataColumn("pclass", "tinyint"));
			Columns.Add(new DataColumn("intprop", "int"));
			Columns.Add(new DataColumn("created", "datetime"));
			Columns.Add(new DataColumn("modified", "datetime"));
		}

		public int ObjectID { get { return Field<int>("id"); } }
		public string Name { get { return Field<string>("name"); } }
		public int NSID { get { return Field<int>("nsid"); } }
		public byte NSClass { get { return Field<byte>("nsclass"); } }
		public int Status { get { return Field<int>("status"); } }
		public string Type { get { return Field<string>("type"); } }
		public int PID{ get { return Field<int>("pid"); } }
		public byte PClass { get { return Field<byte>("pclass"); } }
		public int IntProp { get { return Field<int>("intprop"); } }
		public DateTime Created { get { return Field<DateTime>("created"); } }
		public DateTime Modified { get { return Field<DateTime>("modified"); } }
		
		public override string ToString()
		{
			return Name + " (" + Type + ", " + ObjectID + ")";
		}
	}
}