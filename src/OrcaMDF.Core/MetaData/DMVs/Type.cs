using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Type
	{
		public string Name { get; private set; }
		public byte SystemTypeID { get; private set; }
		public int UserTypeID { get; private set; }
		public int SchemaID { get; private set; }
		public int? PrincipalID { get; private set; }
		public short MaxLength { get; private set; }
		public byte Precision { get; private set; }
		public byte Scale { get; private set; }
		public string CollationName { get { throw new NotImplementedException(); } } // TODO: Get from CollationPropertyFromId(c.cid, 'name')
		public bool IsNullable { get; private set; }
		public bool IsUserDefined { get; private set; }
		public bool IsAssemblyType { get; private set; }
		public int DefaultObjectID { get; private set; }
		public int RuleObjectID { get; private set; }
		public bool IsTableType { get; private set; }

		internal static IEnumerable<Type> GetDmvData(Database db)
		{
			return db.BaseTables.sysscalartypes
				.Select(t => new Type
				    {
						Name = t.name,
				        SystemTypeID = t.xtype,
				        UserTypeID = t.id,
				        SchemaID = t.schid,
				        MaxLength = t.length,
				        Precision = t.prec,
				        Scale = t.scale,
				        DefaultObjectID = t.dflt,
				        RuleObjectID = t.chk,
				        IsNullable = Convert.ToBoolean(1 - (t.status & 1)),
						IsUserDefined = t.id > 256,
						IsAssemblyType = t.xtype == 240,
						IsTableType = t.xtype == 243,
				        PrincipalID = db.BaseTables.syssingleobjrefs
							.Where(o => o.depid == t.id && o.@class == 44 && o.depsubid == 0)
							.Select(o => (int?)o.indepid)
							.SingleOrDefault()
				    });
		}
	}
}