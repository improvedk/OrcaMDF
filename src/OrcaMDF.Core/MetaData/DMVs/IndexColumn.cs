using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class IndexColumn : Row
	{
		private const string CACHE_KEY = "DMV_IndexColumn";

		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("ObjectID", "int"),
				new DataColumn("IndexID", "int"),
				new DataColumn("IndexColumnID", "int"),
				new DataColumn("ColumnID", "int"),
				new DataColumn("KeyOrdinal", "tinyint"),
				new DataColumn("PartitionOrdinal", "tinyint"),
				new DataColumn("IsDescendingKey", "bit", true),
				new DataColumn("IsIncludedColumn", "bit", true)
		    });

		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public int IndexID { get { return Field<int>("IndexID"); } private set { this["IndexID"] = value; } }
		public int IndexColumnID { get { return Field<int>("IndexColumnID"); } private set { this["IndexColumnID"] = value; } }
		public int ColumnID { get { return Field<int>("ColumnID"); } private set { this["ColumnID"] = value; } }
		public byte KeyOrdinal { get { return Field<byte>("KeyOrdinal"); } private set { this["KeyOrdinal"] = value; } }
		public byte PartitionOrdinal { get { return Field<byte>("PartitionOrdinal"); } private set { this["PartitionOrdinal"] = value; } }
		public bool IsDescendingKey { get { return Field<bool>("IsDescendingKey"); } private set { this["IsDescendingKey"] = value; } }
		public bool IsIncludedColumn { get { return Field<bool>("IsIncludedColumn"); } private set { this["IsIncludedColumn"] = value; } }

		public IndexColumn() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new IndexColumn();
		}

		internal static IEnumerable<IndexColumn> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.BaseTables.sysiscols
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
			       	    })
					.ToList();
			}

			return (IEnumerable<IndexColumn>)db.ObjectCache[CACHE_KEY];
		}
	}
}