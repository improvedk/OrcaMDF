using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class IndexColumn
	{
		public int ObjectID { get; private set; }
		public int IndexID { get; private set; }
		public int IndexColumnID { get; private set; }
		public int ColumnID { get; private set; }
		public byte KeyOrdinal { get; private set; }
		public byte PartitionOrdinal { get; private set; }
		public bool IsDescendingKey { get; private set; }
		public bool IsIncludedColumn { get; private set; }

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