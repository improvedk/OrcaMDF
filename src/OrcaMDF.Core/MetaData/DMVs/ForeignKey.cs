using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class ForeignKey : Row
	{
		private const string CACHE_KEY = "DMV_ForeignKey";

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
				new DataColumn("ReferencedObjectID", "int", true),
				new DataColumn("KeyIndexID", "int", true),
				new DataColumn("IsDisabled", "bit"),
				new DataColumn("IsNotForReplication", "bit"),
				new DataColumn("IsNotTrusted", "bit"),
				new DataColumn("DeleteReferentialAction", "tinyint", true),
				new DataColumn("DeleteReferentialActionDesc", "nvarchar", true),
				new DataColumn("UpdateReferentialAction", "tinyint", true),
				new DataColumn("UpdateReferentialActionDesc", "nvarchar", true),
				new DataColumn("IsSystemNamed", "bit")
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
		public int? ReferencedObjectID { get { return Field<int?>("ReferencedObjectID"); } private set { this["ReferencedObjectID"] = value; } }
		public int? KeyIndexID { get { return Field<int?>("KeyIndexID"); } private set { this["KeyIndexID"] = value; } }
		public bool IsDisabled { get { return Field<bool>("IsDisabled"); } private set { this["IsDisabled"] = value; } }
		public bool IsNotForReplication { get { return Field<bool>("IsNotForReplication"); } private set { this["IsNotForReplication"] = value; } }
		public bool IsNotTrusted { get { return Field<bool>("IsNotTrusted"); } private set { this["IsNotTrusted"] = value; } }
		public byte? DeleteReferentialAction { get { return Field<byte?>("DeleteReferentialAction"); } private set { this["DeleteReferentialAction"] = value; } }
		public string DeleteReferentialActionDesc { get { return Field<string>("DeleteReferentialActionDesc"); } private set { this["DeleteReferentialActionDesc"] = value; } }
		public byte? UpdateReferentialAction { get { return Field<byte?>("UpdateReferentialAction"); } private set { this["UpdateReferentialAction"] = value; } }
		public string UpdateReferentialActionDesc { get { return Field<string>("UpdateReferentialActionDesc"); } private set { this["UpdateReferentialActionDesc"] = value; } }
		public bool IsSystemNamed { get { return Field<bool>("IsSystemNamed"); } private set { this["IsSystemNamed"] = value; } }

		public ForeignKey() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new ForeignKey();
		}

		internal static IEnumerable<ForeignKey> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.Dmvs.ObjectsDollar
					.Where(o => o.Type == "F")
					.Select(o => new ForeignKey
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
							ReferencedObjectID = db.BaseTables.syssingleobjrefs
								.Where(f => f.depid == o.ObjectID && f.@class == 27 && f.depsubid == 0)
								.Select(f => f.indepid)
								.Single(),
							KeyIndexID = db.BaseTables.syssingleobjrefs
								.Where(f => f.depid == o.ObjectID && f.@class == 27 && f.depsubid == 0)
								.Select(f => f.indepsubid)
								.Single(),
							IsDisabled = o.IsDisabled,
							IsNotForReplication = o.IsNotForReplication,
							IsNotTrusted = o.IsNotTrusted,
							DeleteReferentialAction = o.DeleteReferentialAction,
							DeleteReferentialActionDesc = db.BaseTables.syspalvalues
								.Where(d => d.@class == "FKRA" && d.value == o.DeleteReferentialAction)
								.Select(d => d.name)
								.Single(),
							UpdateReferentialAction = o.UpdateReferentialAction,
							UpdateReferentialActionDesc = db.BaseTables.syspalvalues
								.Where(u => u.@class == "FKRA" && u.value == o.UpdateReferentialAction)
								.Select(u => u.name)
								.Single(),
							IsSystemNamed = o.IsSystemNamed
						})
					.ToList();
			}

			return (IEnumerable<ForeignKey>)db.ObjectCache[CACHE_KEY];
		}
	}
}