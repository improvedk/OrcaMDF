using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysScalarType : Row
	{
		public SysScalarType()
		{
			Columns.Add(new DataColumn("id", "int"));
			Columns.Add(new DataColumn("schid", "int"));
			Columns.Add(new DataColumn("name", "sysname"));
			Columns.Add(new DataColumn("xtype", "tinyint"));
			Columns.Add(new DataColumn("length", "smallint"));
			Columns.Add(new DataColumn("prec", "tinyint"));
			Columns.Add(new DataColumn("scale", "tinyint"));
			Columns.Add(new DataColumn("collationid", "int"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("created", "datetime"));
			Columns.Add(new DataColumn("modified", "datetime"));
			Columns.Add(new DataColumn("dflt", "int"));
			Columns.Add(new DataColumn("chk", "int"));
		}

		public override Row NewRow()
		{
			return new SysScalarType();
		}

		public int ID { get { return Field<int>("id"); } }
		public int SchemaID { get { return Field<int>("schid"); } }
		public string Name { get { return Field<string>("name"); } }
		public byte XType { get { return Field<byte>("xtype"); } }
		public short Length { get { return Field<short>("length"); } }
		public byte Precision { get { return Field<byte>("prec"); } }
		public byte Scale { get { return Field<byte>("scale"); } }
		public int CollationID { get { return Field<int>("collationid"); } }
		public int Status { get { return Field<int>("status"); } }
		public DateTime Created { get { return Field<DateTime>("created"); } }
		public DateTime Modified { get { return Field<DateTime>("modified"); } }
		public int Dflt { get { return Field<int>("dflt"); } }
		public int Chk { get { return Field<int>("chk"); } }

		// Calculated fields
		public bool IsNullable { get { return Convert.ToBoolean(1 - (Status & 1)); } } 
		public bool AnsiPadded { get { return Convert.ToBoolean(Status & 2); } } 
		public bool IsRowGuidCol { get { return Convert.ToBoolean(Status & 8); } } 
		public bool IsIdentity { get { return Convert.ToBoolean(Status & 4); } } 
		public bool IsComputed { get { return Convert.ToBoolean(Status & 16); } } 
		public bool IsFilestream { get { return Convert.ToBoolean(Status & 32); } } 
		public bool IsReplicated { get { return Convert.ToBoolean(Status & 0x020000); } } 
		public bool IsNonSqlSubscribed { get { return Convert.ToBoolean(Status & 0x040000); } } 
		public bool IsMergePublished { get { return Convert.ToBoolean(Status & 0x080000); } } 
		public bool IsDtsReplicated { get { return Convert.ToBoolean(Status & 0x100000); } }
		public bool IsXmlDocument { get { return Convert.ToBoolean(Status & 2048); } }
	}
}