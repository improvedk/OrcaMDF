using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Object : Row
	{
		private const string CACHE_KEY = "DMV_Object";

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
				new DataColumn("IsSchemaPublished", "bit")
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

		public Object() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new Object();
		}

		internal static IEnumerable<Object> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.Dmvs.ObjectsDollar
					.Select(o => new Object
						{
							Name = o.Name,
							ObjectID = o.ObjectID,
							PrincipalID = o.PrincipalID,
							SchemaID = o.SchemaID,
							ParentObjectID = o.ParentObjectID,
							Type = o.Type.Trim(),
							TypeDesc = o.TypeDesc,
							CreateDate = o.CreateDate,
							ModifyDate = o.ModifyDate,
							IsMSShipped = o.IsMSShipped,
							IsPublished = o.IsPublished,
							IsSchemaPublished = o.IsSchemaPublished
						})
					.ToList();
			}

			return (IEnumerable<Object>)db.ObjectCache[CACHE_KEY];
		}
	}
}