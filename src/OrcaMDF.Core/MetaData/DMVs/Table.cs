using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Table : Row
	{
		private const string CACHE_KEY = "DMV_Table";

		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("Name", "sysname"),
				new DataColumn("ObjectID", "int"),
				new DataColumn("PrincipalID", "int", true),
				new DataColumn("SchemaID", "int"),
				new DataColumn("ParentObjectID", "int"),
				new DataColumn("Type", "char(2)"),
				new DataColumn("TypeDesc", "nvarchar", true),
				new DataColumn("CreateDate", "datetime"),
				new DataColumn("ModifyDate", "datetime"),
				new DataColumn("IsMSShipped", "bit"),
				new DataColumn("IsPublished", "bit"),
				new DataColumn("IsSchemaPublished", "bit"),
				new DataColumn("LobDataSpaceID", "int", true),
				new DataColumn("FilestreamDataSpaceID", "int", true),
				new DataColumn("MaxColumnIDUsed", "int"),
				new DataColumn("LockOnBulkLoad", "bit"),
				new DataColumn("UsesAnsiNulls", "bit", true),
				new DataColumn("IsReplicated", "bit", true),
				new DataColumn("HasReplicationFilter", "bit", true),
				new DataColumn("IsMergePublished", "bit", true),
				new DataColumn("IsSyncTranSubscribed", "bit", true),
				new DataColumn("HasUncheckedAssemblyData", "bit"),
				new DataColumn("TextInRowLimit", "int", true),
				new DataColumn("LargeValueTypesOutOfRow", "bit", true),
				new DataColumn("IsTrackedByCdc", "bit", true),
				new DataColumn("LockEscalation", "tinyint", true),
				new DataColumn("LockEscalationDesc", "nvarchar", true)
		    });

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

		public Table() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new Table();
		}

		internal static IEnumerable<Table> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.Dmvs.ObjectsDollar
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
					    })
					.ToList();
			}

			return (IEnumerable<Table>)db.ObjectCache[CACHE_KEY];
		}
	}
}