using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Table : Row
	{
		public string Name { get { return Field<string>("Name"); } private set { this["Name"] = value; } }
		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public int? PrincipalID { get { return Field<int?>("PrincipalID"); } private set { this["PrincipalID"] = value; } }
		public int SchemaID { get { return Field<int>("SchemaID"); } private set { this["SchemaID"] = value; } }
		public int ParentObjectID { get { return Field<int>("ParentObjectID"); } private set { this["ParentObjectID"] = value; } }
		public string Type { get { return Field<string>("Type"); } private set { this["Type"] = value; } }
		public string TypeDesc { get { return Field<string>("TypeDesc"); } private set { this["TypeDesc"] = value; } }
		public DateTime CreateDate { get { return Field<DateTime>("CreateDate"); } private set { this["CreateDate"] = value; } }
		public DateTime ModifyDate { get { return Field<DateTime>("ModifyDate"); } private set { this["ModifyDate"] = value; } }
		public bool IsMSShipped { get { return Field<bool>("IsMSShipped"); } private set { this["IsMSShipped"] = value; } }
		public bool IsPublished { get { return Field<bool>("IsPublished"); } private set { this["IsPublished"] = value; } }
		public bool IsSchemaPublished { get { return Field<bool>("IsSchemaPublished"); } private set { this["IsSchemaPublished"] = value; } }
		public int? LobDataSpaceID { get { return Field<int?>("LobDataSpaceID"); } private set { this["LobDataSpaceID"] = value; } }
		public int? FilestreamDataSpaceID { get { return Field<int?>("FilestreamDataSpaceID"); } private set { this["FilestreamDataSpaceID"] = value; } }
		public int MaxColumnIDUsed { get { return Field<int>("MaxColumnIDUsed"); } private set { this["MaxColumnIDUsed"] = value; } }
		public bool LockOnBulkLoad { get { return Field<bool>("LockOnBulkLoad"); } private set { this["LockOnBulkLoad"] = value; } }
		public bool UsesAnsiNulls { get { return Field<bool>("UsesAnsiNulls"); } private set { this["UsesAnsiNulls"] = value; } }
		public bool IsReplicated { get { return Field<bool>("IsReplicated"); } private set { this["IsReplicated"] = value; } }
		public bool HasReplicationFilter { get { return Field<bool>("HasReplicationFilter"); } private set { this["HasReplicationFilter"] = value; } }
		public bool IsMergePublished { get { return Field<bool>("IsMergePublished"); } private set { this["IsMergePublished"] = value; } }
		public bool IsSyncTranSubscribed { get { return Field<bool>("IsSyncTranSubscribed"); } private set { this["IsSyncTranSubscribed"] = value; } }
		public bool HasUncheckedAssemblyData { get { return Field<bool>("HasUncheckedAssemblyData"); } private set { this["HasUncheckedAssemblyData"] = value; } }
		public int? TextInRowLimit { get { return Field<int?>("TextInRowLimit"); } private set { this["TextInRowLimit"] = value; } }
		public bool LargeValueTypesOutOfRow { get { return Field<bool>("LargeValueTypesOutOfRow"); } private set { this["LargeValueTypesOutOfRow"] = value; } }
		public bool IsTrackedByCdc { get { return Field<bool>("IsTrackedByCdc"); } private set { this["IsTrackedByCdc"] = value; } }
		public byte LockEscalation { get { return Field<byte>("LockEscalation"); } private set { this["LockEscalation"] = value; } }
		public string LockEscalationDesc { get { return Field<string>("LockEscalationDesc"); } private set { this["LockEscalationDesc"] = value; } }

		public Table()
		{
			Columns.Add(new DataColumn("Name", "sysname"));
			Columns.Add(new DataColumn("ObjectID", "int"));
			Columns.Add(new DataColumn("PrincipalID", "int", true));
			Columns.Add(new DataColumn("SchemaID", "int"));
			Columns.Add(new DataColumn("ParentObjectID", "int"));
			Columns.Add(new DataColumn("Type", "char(2)"));
			Columns.Add(new DataColumn("TypeDesc", "nvarchar", true));
			Columns.Add(new DataColumn("CreateDate", "datetime"));
			Columns.Add(new DataColumn("ModifyDate", "datetime"));
			Columns.Add(new DataColumn("IsMSShipped", "bit"));
			Columns.Add(new DataColumn("IsPublished", "bit"));
			Columns.Add(new DataColumn("IsSchemaPublished", "bit"));
			Columns.Add(new DataColumn("LobDataSpaceID", "int", true));
			Columns.Add(new DataColumn("FilestreamDataSpaceID", "int", true));
			Columns.Add(new DataColumn("MaxColumnIDUsed", "int"));
			Columns.Add(new DataColumn("LockOnBulkLoad", "bit"));
			Columns.Add(new DataColumn("UsesAnsiNulls", "bit", true));
			Columns.Add(new DataColumn("IsReplicated", "bit", true));
			Columns.Add(new DataColumn("HasReplicationFilter", "bit", true));
			Columns.Add(new DataColumn("IsMergePublished", "bit", true));
			Columns.Add(new DataColumn("IsSyncTranSubscribed", "bit", true));
			Columns.Add(new DataColumn("HasUncheckedAssemblyData", "bit"));
			Columns.Add(new DataColumn("TextInRowLimit", "int", true));
			Columns.Add(new DataColumn("LargeValueTypesOutOfRow", "bit", true));
			Columns.Add(new DataColumn("IsTrackedByCdc", "bit", true));
			Columns.Add(new DataColumn("LockEscalation", "tinyint", true));
			Columns.Add(new DataColumn("LockEscalationDesc", "nvarchar", true));
		}

		public override Row NewRow()
		{
			return new Table();
		}

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