using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class ObjectDollar
	{
		public string Name { get; private set; }
		public int ObjectID { get; private set; }
		public int? PrincipalID { get; private set; }
		public int SchemaID { get; private set; }
		public int ParentObjectID { get; private set; }
		public string Type { get; private set; }
		public string TypeDesc { get; private set; }
		public int Property { get; private set; }
		public DateTime CreateDate { get; private set; }
		public DateTime ModifyDate { get; private set; }
		public bool IsMSShipped { get; private set; }
		public bool IsAutoDropped { get; private set; }
		public bool IsSystemNamed { get; private set; }
		public bool IsPublished { get; private set; }
		public bool IsSchemaPublished { get; private set; }
		public bool LockOnBulkLoad { get; private set; }
		public bool IsDisabled { get; private set; }
		public bool IsAutoExecuted { get; private set; }
		public bool IsActivationEnabled { get; private set; }
		public bool HasOpaqueMetadata { get; private set; }
		public bool IsNotForReplication { get; private set; }
		public bool IsReceiveEnabled { get; private set; }
		public bool IsNotTrusted { get; private set; }
		public bool IsEnqueueEnabled { get; private set; }
		public bool WithCheckOption { get; private set; }
		public bool IsRetentionEnabled { get; private set; }
		public bool HasUncheckedAssemblyData { get; private set; }
		public byte UpdateReferentialAction { get; private set; }
		public byte DeleteReferentialAction { get; private set; }
		public bool IsReplicated { get; private set; }
		public bool IsExecutionReplicated { get; private set; }
		public bool HasReplicationFilter { get; private set; }
		public bool IsReplSerializableOnly { get; private set; }
		public bool IsMergePublished { get; private set; }
		public bool SkipsReplConstraints { get; private set; }
		public bool IsSyncTranSubscribed { get; private set; }
		public bool UsesAnsiNulls { get; private set; }
		public bool NullOnNullInput { get; private set; }
		public bool UsesDatabaseCollation { get; private set; }
		public bool IsTrackedByCdc { get; private set; }
		public bool LargeValueTypesOutOfRow { get; private set; }
		public byte LockEscalationOption { get; private set; }
		public bool IsPoisonMessageHandlingEnabled { get; private set; }

		internal static IEnumerable<ObjectDollar> GetDmvData(Database db)
		{
			return db.BaseTables.sysschobjs
				.Where(o => o.nsclass == 0 && o.pclass == 1)
				.Select(o => new ObjectDollar
					{
						Name = o.name,
						ObjectID = o.id,
						PrincipalID = db.BaseTables.syssingleobjrefs
							.Where(r => r.depid == o.id && r.@class == 97 && r.depsubid == 0)
							.Select(r => r.indepid)
							.SingleOrDefault(),
						SchemaID = o.nsid,
						ParentObjectID = o.pid,
						Type = o.type.Trim(),
						TypeDesc = db.BaseTables.syspalnames
							.Where(n => n.@class == "OBTY" && n.value.Trim() == o.type.Trim())
							.Select(n => n.name)
							.Single(),
						Property = o.intprop,
						CreateDate = o.created,
						ModifyDate = o.modified,
						IsMSShipped = Convert.ToBoolean(o.status & 1),
						IsAutoDropped = Convert.ToBoolean(o.status & 2),
						IsSystemNamed = Convert.ToBoolean(o.status & 4),
						IsPublished = Convert.ToBoolean(o.status & 16),
						IsSchemaPublished = Convert.ToBoolean(o.status & 64),
						LockOnBulkLoad = Convert.ToBoolean(o.status & 256),
						IsDisabled = Convert.ToBoolean(o.status & 256),
						IsAutoExecuted = Convert.ToBoolean(o.status & 256),
						IsActivationEnabled = Convert.ToBoolean(o.status & 256),
						HasOpaqueMetadata = Convert.ToBoolean(o.status & 512),
						IsNotForReplication = Convert.ToBoolean(o.status & 512),
						IsReceiveEnabled = Convert.ToBoolean(o.status & 512),
						IsNotTrusted = Convert.ToBoolean(o.status & 1024),
						IsEnqueueEnabled = Convert.ToBoolean(o.status & 1024),
						WithCheckOption = Convert.ToBoolean(o.status & 1024),
						IsRetentionEnabled = Convert.ToBoolean(o.status & 2048),
						HasUncheckedAssemblyData = Convert.ToBoolean(o.status & 2048),
						UpdateReferentialAction = Convert.ToByte((o.status / 4096) & 3),
						DeleteReferentialAction = Convert.ToByte((o.status / 16384) & 3),
						IsReplicated = Convert.ToBoolean(o.status & 0x1000),
						IsExecutionReplicated = Convert.ToBoolean(o.status & 0x1000),
						HasReplicationFilter = Convert.ToBoolean(o.status & 0x2000),
						IsReplSerializableOnly = Convert.ToBoolean(o.status & 0x2000),
						IsMergePublished = Convert.ToBoolean(o.status & 0x4000),
						SkipsReplConstraints = Convert.ToBoolean(o.status & 0x4000),
						IsSyncTranSubscribed = Convert.ToBoolean(o.status & 0x8000),
						UsesAnsiNulls = Convert.ToBoolean(o.status & 0x40000),
						NullOnNullInput = Convert.ToBoolean(o.status & 0x200000),
						UsesDatabaseCollation = Convert.ToBoolean(o.status & 0x100000),
						IsTrackedByCdc = Convert.ToBoolean(o.status & 0x01000000),
						LargeValueTypesOutOfRow = Convert.ToBoolean(o.status & 0x02000000),
						LockEscalationOption = Convert.ToByte((o.status & 0x30000000) / 0x10000000),
						IsPoisonMessageHandlingEnabled = Convert.ToBoolean(o.status & 0x04000000)
					});
		}
	}
}