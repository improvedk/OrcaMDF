using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Index
	{
		public int ObjectID { get; private set; }
		public string Name { get; private set; }
		public int IndexID { get; private set; }
		public byte Type { get; private set; }
		public string TypeDesc { get; private set; }
		public int DataSpaceID { get; private set; }
		public bool? IgnoreDupKey { get; private set; }
		public byte FillFactor { get; private set; }
		public bool IsUnique { get; private set; }
		public bool IsPrimaryKey { get; private set; }
		public bool IsUniqueConstraint { get; private set; }
		public bool IsPadded { get; private set; }
		public bool IsDisabled { get; private set; }
		public bool IsHypothetical { get; private set; }
		public bool AllowRowLocks { get; private set; }
		public bool AllowPageLocks { get; private set; }
		public bool HasFilter { get; private set; }
		public string FilterDefinition { get { throw new NotImplementedException(); } } // TODO: Get value from OBJECT_DEFINITION (case when (i.status & 0x20000) != 0 then object_definition(i.id, i.indid, 7) else NULL end)

		internal static IEnumerable<Index> GetDmvData(Database db)
		{
			return db.BaseTables.sysidxstats
				.Where(i => (i.status & 1) != 0)
				.Select(i => new Index
				    {
						ObjectID = i.id,
						Name = i.name,
						IndexID = i.indid,
						Type = i.type,
						TypeDesc = db.BaseTables.syspalvalues
							.Where(n => n.@class == "IDXT" && n.value == i.type)
							.Select(n => n.name)
							.Single(),
						DataSpaceID = i.dataspace,
						FillFactor = i.fillfact,
						IsUnique = Convert.ToBoolean(i.status & 0x8),
						IsPrimaryKey = Convert.ToBoolean(i.status & 0x20),
						IsUniqueConstraint = Convert.ToBoolean(i.status & 0x40),
						IsPadded = Convert.ToBoolean(i.status & 0x10),
						IsDisabled = Convert.ToBoolean(i.status & 0x80),
						IsHypothetical = Convert.ToBoolean(i.status & 0x100),
						AllowRowLocks = Convert.ToBoolean(1 - (i.status & 512) / 512),
						AllowPageLocks = Convert.ToBoolean(1 - (i.status & 1024) / 1024),
						HasFilter = Convert.ToBoolean(i.status & 0x20000)
				    });
		}
	}
}