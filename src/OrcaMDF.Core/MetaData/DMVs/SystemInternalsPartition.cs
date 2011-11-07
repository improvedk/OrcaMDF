using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SystemInternalsPartition
	{
		public long PartitionID { get; private set; }
		public int ObjectID { get; private set; }
		public int IndexID { get; private set; }
		public int PartitionNumber { get; private set; }
		public long Rows { get { throw new NotImplementedException(); } } // TODO: Get value from OpenRowset(TABLE ALUCOUNT, rs.rowsetid, 0) ct
		public short FilestreamFilegroupID { get; private set; }
		public bool IsOrphaned { get; private set; }
		public byte DroppedLobColumnState { get; private set; }
		public bool IsUnique { get; private set; }
		public bool IsReplicated { get; private set; }
		public bool IsLoggedForReplication { get; private set; }
		public short MaxNullBitUsed { get; private set; }
		public int MaxLeafLength { get; private set; }
		public short MinLeafLength { get; private set; }
		public short MaxInternalLength { get; private set; }
		public short MinInternalLength { get; private set; }
		public bool AllowsNullableKeys { get; private set; }
		public bool AllowRowLocks { get; private set; }
		public bool AllowPageLocks { get; private set; }
		public bool IsDataRowFormat { get; private set; }
		public bool IsNotVersioned { get; private set; }
		public Guid? FilestreamGuid { get; private set; }

		internal static IEnumerable<SystemInternalsPartition> GetDmvData(Database db)
		{
			return db.BaseTables.sysrowsets
				.Select(rs => new SystemInternalsPartition
				{
					PartitionID = rs.rowsetid,
					ObjectID = rs.idmajor,
					IndexID = rs.idminor,
					PartitionNumber = rs.numpart,
					FilestreamFilegroupID = rs.fgidfs,
					MaxNullBitUsed = rs.maxnullbit,
					MaxLeafLength = rs.maxleaf,
					MinLeafLength = rs.minleaf,
					MaxInternalLength = rs.maxint,
					MinInternalLength = rs.minint,
					FilestreamGuid = rs.rsguid != null ? (Guid?)(new Guid(rs.rsguid)) : null,
					IsOrphaned = Convert.ToBoolean(1 - rs.ownertype),
					DroppedLobColumnState = Convert.ToByte(rs.status & 3),
					IsUnique = Convert.ToBoolean(rs.status & 4),
					IsReplicated = Convert.ToBoolean(rs.status & 8),
					IsLoggedForReplication = Convert.ToBoolean(rs.status & 16),
					AllowsNullableKeys = Convert.ToBoolean(rs.status & 64),
					AllowRowLocks = Convert.ToBoolean(1 - (rs.status & 256) / 256),
					AllowPageLocks = Convert.ToBoolean(1 - (rs.status & 256) / 256),
					IsDataRowFormat = Convert.ToBoolean(rs.status & 512),
					IsNotVersioned = Convert.ToBoolean(rs.status & 2048)
				});
		}
	}
}