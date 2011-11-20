using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SystemInternalsPartition : Row
	{
		public long PartitionID { get { return Field<long>("PartitionID"); } private set { this["PartitionID"] = value; } }
		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public int IndexID { get { return Field<int>("IndexID"); } private set { this["IndexID"] = value; } }
		public int PartitionNumber { get { return Field<int>("PartitionNumber"); } private set { this["PartitionNumber"] = value; } }
		public long? Rows { get { return Field<long?>("Rows"); } private set { this["Rows"] = value; } } // TODO
		public short FilestreamFilegroupID { get { return Field<short>("FilestreamFilegroupID"); } private set { this["FilestreamFilegroupID"] = value; } }
		public bool IsOrphaned { get { return Field<bool>("IsOrphaned"); } private set { this["IsOrphaned"] = value; } }
		public byte DroppedLobColumnState { get { return Field<byte>("DroppedLobColumnState"); } private set { this["DroppedLobColumnState"] = value; } }
		public bool IsUnique { get { return Field<bool>("IsUnique"); } private set { this["IsUnique"] = value; } }
		public bool IsReplicated { get { return Field<bool>("IsReplicated"); } private set { this["IsReplicated"] = value; } }
		public bool IsLoggedForReplication { get { return Field<bool>("IsLoggedForReplication"); } private set { this["IsLoggedForReplication"] = value; } }
		public short MaxNullBitUsed { get { return Field<short>("MaxNullBitUsed"); } private set { this["MaxNullBitUsed"] = value; } }
		public int MaxLeafLength { get { return Field<int>("MaxLeafLength"); } private set { this["MaxLeafLength"] = value; } }
		public short MinLeafLength { get { return Field<short>("MinLeafLength"); } private set { this["MinLeafLength"] = value; } }
		public short MaxInternalLength { get { return Field<short>("MaxInternalLength"); } private set { this["MaxInternalLength"] = value; } }
		public short MinInternalLength { get { return Field<short>("MinInternalLength"); } private set { this["MinInternalLength"] = value; } }
		public bool AllowsNullableKeys { get { return Field<bool>("AllowsNullableKeys"); } private set { this["AllowsNullableKeys"] = value; } }
		public bool AllowRowLocks { get { return Field<bool>("AllowRowLocks"); } private set { this["AllowRowLocks"] = value; } }
		public bool AllowPageLocks { get { return Field<bool>("AllowPageLocks"); } private set { this["AllowPageLocks"] = value; } }
		public bool IsDataRowFormat { get { return Field<bool>("IsDataRowFormat"); } private set { this["IsDataRowFormat"] = value; } }
		public bool IsNotVersioned { get { return Field<bool>("IsNotVersioned"); } private set { this["IsNotVersioned"] = value; } }
		public Guid? FilestreamGuid { get { return Field<Guid?>("FilestreamGuid"); } private set { this["FilestreamGuid"] = value; } }

		public SystemInternalsPartition()
		{
			Columns.Add(new DataColumn("PartitionID", "bigint"));
			Columns.Add(new DataColumn("ObjectID", "int"));
			Columns.Add(new DataColumn("IndexID", "int"));
			Columns.Add(new DataColumn("PartitionNumber", "int"));
			Columns.Add(new DataColumn("Rows", "bigint"));
			Columns.Add(new DataColumn("FilestreamFilegroupID", "smallint"));
			Columns.Add(new DataColumn("IsOrphaned", "bit", true));
			Columns.Add(new DataColumn("DroppedLobColumnState", "tinyint", true));
			Columns.Add(new DataColumn("IsUnique", "bit", true));
			Columns.Add(new DataColumn("IsReplicated", "bit", true));
			Columns.Add(new DataColumn("IsLoggedForReplication", "bit", true));
			Columns.Add(new DataColumn("MaxNullBitUsed", "smallint"));
			Columns.Add(new DataColumn("MaxLeafLength", "int"));
			Columns.Add(new DataColumn("MinLeafLength", "smallint"));
			Columns.Add(new DataColumn("MaxInternalLength", "smallint"));
			Columns.Add(new DataColumn("MinInternalLength", "smallint"));
			Columns.Add(new DataColumn("AllowsNullableKeys", "bit", true));
			Columns.Add(new DataColumn("AllowRowLocks", "bit", true));
			Columns.Add(new DataColumn("AllowPageLocks", "bit", true));
			Columns.Add(new DataColumn("IsDataRowFormat", "bit", true));
			Columns.Add(new DataColumn("IsNotVersioned", "bit", true));
			Columns.Add(new DataColumn("FilestreamGuid", "uniqueidentifier", true));
		}

		public override Row NewRow()
		{
			return new SystemInternalsPartition();
		}

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