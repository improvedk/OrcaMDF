using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Object
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

		internal static IEnumerable<Object> GetDmvData(Database db)
		{
			// SQL Server gets data from sys.objects$. We need to get directly from sys.sysschobjs to avoid a stack overflow
			return db.Dmvs.ObjectsDollar
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
				    });
					
			//return db.BaseTables.sysschobjs
			//    .Select(o => new Object
			//        {
			//            Name = o.name,
			//            ObjectID = o.id,
			//            PrincipalID = db.BaseTables.syssingleobjrefs
			//                .Where(r => r.depid == o.id && r.@class == 97 && r.depsubid == 0)
			//                .Select(r => r.indepid)
			//                .SingleOrDefault(),
			//            SchemaID = o.nsid,
			//            ParentObjectID = o.pid,
			//            Type = o.type.Trim(),
			//            TypeDesc = db.BaseTables.syspalnames
			//                .Where(n => n.@class == "OBTY" && n.value.Trim() == o.type.Trim())
			//                .Select(n => n.name)
			//                .Single(),
			//            CreateDate = o.created,
			//            ModifyDate = o.modified,
			//            IsMSShipped = Convert.ToBoolean(o.status & 1),
			//            IsPublished = Convert.ToBoolean(o.status & 16),
			//            IsSchemaPublished = Convert.ToBoolean(o.status & 64)
			//        });
		}
	}
}