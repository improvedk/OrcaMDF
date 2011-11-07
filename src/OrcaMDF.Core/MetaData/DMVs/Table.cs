using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	// TODO: Remove nullabes where not needed
	public class Table
	{
		public string Name { get; private set; }
		public int ObjectID { get; private set; }
		public int? PrincipalID { get; private set; }
		public int SchemaID { get; private set; }
		public int ParentObjectID { get; private set; }
		public string Type { get; private set; }
		public string TypeDesc { get; private set; }
		public DateTime CreateDate { get; private set; }
		public DateTime ModifyDate { get; private set; }
		public bool IsMSShipped { get; private set; }
		public bool IsPublished { get; private set; }
		public bool IsSchemaPublished { get; private set; }
		public int? LobDataSpaceID { get; private set; }
		public int? FilestreamDataSpaceID { get; private set; }
		public int MaxColumnIDUsed { get; private set; }
		public bool LockOnBulkLoad { get; private set; }
		public bool? UsesAnsiNulls { get; private set; }
		public bool? IsReplicated { get; private set; }
		public bool? HasReplicationFilter { get; private set; }
		public bool? IsMergePublished { get; private set; }
		public bool? IsSyncTranSubscribed { get; private set; }
		public bool HasUncheckedAssemblyData { get; private set; }
		public int? TextInRowLimit { get; private set; }
		public bool? LargeValueTypesOutOfRow { get; private set; }
		public bool? IsTrackedByCdc { get; private set; }
		public byte? LockEscalation { get; private set; }
		public string LockEscalationDesc { get; private set; }

		internal static IEnumerable<Table> GetDmvData(Database db)
		{
			return db.Dmvs.ObjectsDollar
				.Where(o => o.Type == "U")
				.Select(o => new Table
				    {
						Name = o.Name,
						ObjectID = o.ObjectID,
						PrincipalID = o.PrincipalID,
						SchemaID = o.SchemaID,
						ParentObjectID = o.ParentObjectID,
						Type = o.Type,
						TypeDesc = o.TypeDesc,
						CreateDate = o.CreateDate,
						ModifyDate = o.ModifyDate,
						IsMSShipped = o.IsMSShipped,
						IsPublished = o.IsPublished,
						IsSchemaPublished = o.IsSchemaPublished,
						LobDataSpaceID = db.BaseTables.sysidxstats
							.Where(lob => lob.id == o.ObjectID && lob.indid <= 1)
							.Select(lob => (int?)lob.lobds)
							.SingleOrDefault(),
						FilestreamDataSpaceID = db.BaseTables.syssingleobjrefs
							.Where(rfs => rfs.depid == o.ObjectID && rfs.@class == 42 && rfs.depsubid == 0)
							.Select(rfs => (int?)rfs.indepid)
							.SingleOrDefault(),
						MaxColumnIDUsed = o.Property,
						LockOnBulkLoad = o.LockOnBulkLoad,
						UsesAnsiNulls = o.UsesAnsiNulls,
						IsReplicated = o.IsReplicated,
						HasReplicationFilter = o.HasReplicationFilter,
						IsMergePublished = o.IsMergePublished,
						IsSyncTranSubscribed = o.IsSyncTranSubscribed,
						HasUncheckedAssemblyData = o.HasUncheckedAssemblyData,
						TextInRowLimit = db.BaseTables.sysidxstats
							.Where(lob => lob.id == o.ObjectID && lob.indid <= 1)
							.Select(lob => (int?)lob.intprop)
							.SingleOrDefault(),
						LargeValueTypesOutOfRow = o.LargeValueTypesOutOfRow,
						IsTrackedByCdc = o.IsTrackedByCdc,
						LockEscalation = o.LockEscalationOption,
						LockEscalationDesc = db.BaseTables.syspalvalues
							.Where(ts => ts.@class == "LEOP" && ts.value == o.LockEscalationOption)
							.Select(ts => ts.name)
							.Single()
				    });
		}
	}
}