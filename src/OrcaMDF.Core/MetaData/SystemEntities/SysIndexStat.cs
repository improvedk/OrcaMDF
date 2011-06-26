using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysIndexStat : Row
	{
		public SysIndexStat()
		{
			Columns.Add(new DataColumn("id", "int"));
			Columns.Add(new DataColumn("indid", "int"));
			Columns.Add(new DataColumn("name", "sysname", true));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("intprop", "int"));
			Columns.Add(new DataColumn("fillfact", "tinyint"));
			Columns.Add(new DataColumn("type", "tinyint"));
			Columns.Add(new DataColumn("tinyprop", "tinyint"));
			Columns.Add(new DataColumn("dataspace", "int"));
			Columns.Add(new DataColumn("lobds", "int"));
			Columns.Add(new DataColumn("rowset", "bigint"));
		}

		public override Row NewRow()
		{
			return new SysIndexStat();
		}

		public int ObjectID { get { return Field<int>("id"); } }
		public int IndexID { get { return Field<int>("indid"); } }
		public string Name { get { return Field<string>("name"); } }
		public int Status { get { return Field<int>("status"); } }
		public int IntProp { get { return Field<int>("intprop"); } }
		public short FillFactor { get { return Field<short>("fillfact"); } }
		public short Type { get { return Field<short>("type"); } }
		public short TinyProp { get { return Field<short>("tinyprop"); } }
		public int DataSpaceID { get { return Field<int>("dataspace"); } }
		public int LobDS { get { return Field<int>("lobds"); } }
		public long Rowset { get { return Field<long>("rowset"); } }

		// Calculated fields
		public bool IsUnique { get { return Convert.ToBoolean(Status & 0x8); } }
		public bool IgnoreDuplicateKey { get { return Convert.ToBoolean(Status & 0x4); } }
		public bool IsPrimaryKey { get { return Convert.ToBoolean(Status & 0x20); } }
		public bool IsUniqueConstraint { get { return Convert.ToBoolean(Status & 0x40); } }
		public bool IsPadded { get { return Convert.ToBoolean(Status & 0x10); } }
		public bool IsDisabled { get { return Convert.ToBoolean(Status & 0x80); } }
		public bool IsHypotehtical { get { return Convert.ToBoolean(Status & 0x100); } }
		public bool AllowRowLocks { get { return Convert.ToBoolean(1 - (Status & 512) / 512); } }
		public bool AllowPageLocks { get { return Convert.ToBoolean(1 - (Status & 1024) / 1024); } }
		public bool HasFilter { get { return Convert.ToBoolean(Status & 0x20000); } }
	}
}