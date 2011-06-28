using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysIsCol : Row
	{
		public SysIsCol()
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
			return new SysIsCol();
		}

		public int ObjectID { get { return Field<int>("idmajor"); } }
		public int IndexID { get { return Field<int>("idminor"); } }
		public int IndexColumnID { get { return Field<int>("subid"); } }
		public int ColumnID { get { return Field<int>("intprop"); } }
		public short KeyOrdinal { get { return Field<short>("tinyprop1"); } }
		public short PartitionOrdinal { get { return Field<short>("tinyprop2"); } }

		// Calculated fields
		public bool IsDescendingKey { get { return Convert.ToBoolean(Field<int>("status") & 0x4); } }
		public bool IsIncluded { get { return Convert.ToBoolean(Field<int>("status") & 0x10); } }

		// TODO: Needs to determine what (status & 2) predicate signifies in sys.index_columns
	}
}