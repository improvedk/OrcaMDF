using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class ForeignKey
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
		public int? ReferencedObjectID { get; private set; }
		public int? KeyIndexID { get; private set; }
		public bool IsDisabled { get; private set; }
		public bool IsNotForReplication { get; private set; }
		public bool IsNotTrusted { get; private set; }
		public byte? DeleteReferentialAction { get; private set; }
		public string DeleteReferentialActionDesc { get; private set; }
		public byte? UpdateReferentialAction { get; private set; }
		public string UpdateReferentialActionDesc { get; private set; }
		public bool IsSystemNamed { get; private set; }

		internal static IEnumerable<ForeignKey> GetDmvData(Database db)
		{
			return db.Dmvs.ObjectsDollar
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
						KeyIndexID= db.BaseTables.syssingleobjrefs
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
				    });
		}
	}
}