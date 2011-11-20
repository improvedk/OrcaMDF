using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class IndexColumn : Row
	{
		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public int IndexID { get { return Field<int>("IndexID"); } private set { this["IndexID"] = value; } }
		public int IndexColumnID { get { return Field<int>("IndexColumnID"); } private set { this["IndexColumnID"] = value; } }
		public int ColumnID { get { return Field<int>("ColumnID"); } private set { this["ColumnID"] = value; } }
		public byte KeyOrdinal { get { return Field<byte>("KeyOrdinal"); } private set { this["KeyOrdinal"] = value; } }
		public byte PartitionOrdinal { get { return Field<byte>("PartitionOrdinal"); } private set { this["PartitionOrdinal"] = value; } }
		public bool IsDescendingKey { get { return Field<bool>("IsDescendingKey"); } private set { this["IsDescendingKey"] = value; } }
		public bool IsIncludedColumn { get { return Field<bool>("IsIncludedColumn"); } private set { this["IsIncludedColumn"] = value; } }

		public IndexColumn()
		{
			Columns.Add(new DataColumn("ObjectID", "int"));
			Columns.Add(new DataColumn("IndexID", "int"));
			Columns.Add(new DataColumn("IndexColumnID", "int"));
			Columns.Add(new DataColumn("ColumnID", "int"));
			Columns.Add(new DataColumn("KeyOrdinal", "tinyint"));
			Columns.Add(new DataColumn("PartitionOrdinal", "tinyint"));
			Columns.Add(new DataColumn("IsDescendingKey", "bit", true));
			Columns.Add(new DataColumn("IsIncludedColumn", "bit", true));
		}

		public override Row NewRow()
		{
			return new IndexColumn();
		}

		internal static IEnumerable<IndexColumn> GetDmvData(Database db)
		{
			return db.BaseTables.sysiscols
				.Where(ic => (ic.status & 2) != 0)
				.Select(ic => new IndexColumn
				{
					ObjectID = ic.idmajor,
					IndexID = ic.idminor,
					IndexColumnID = ic.subid,
					ColumnID = ic.intprop,
					KeyOrdinal = ic.tinyprop1,
					PartitionOrdinal = ic.tinyprop2,
					IsDescendingKey = Convert.ToBoolean(ic.status & 0x4),
					IsIncludedColumn = Convert.ToBoolean(ic.status & 0x10)
				});
		}
	}
}