using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Type : Row
	{
		public string Name { get { return Field<string>("Name"); } private set { this["Name"] = value; } }
		public byte SystemTypeID { get { return Field<byte>("SystemTypeID"); } private set { this["SystemTypeID"] = value; } }
		public int UserTypeID { get { return Field<int>("UserTypeID"); } private set { this["UserTypeID"] = value; } }
		public int SchemaID { get { return Field<int>("SchemaID"); } private set { this["SchemaID"] = value; } }
		public int? PrincipalID { get { return Field<int?>("PrincipalID"); } private set { this["PrincipalID"] = value; } }
		public short MaxLength { get { return Field<short>("MaxLength"); } private set { this["MaxLength"] = value; } }
		public byte Precision { get { return Field<byte>("Precision"); } private set { this["Precision"] = value; } }
		public byte Scale { get { return Field<byte>("Scale"); } private set { this["Scale"] = value; } }
		public string CollationName { get { return Field<string>("CollationName"); } private set { this["CollationName"] = value; } } // TODO
		public bool IsNullable { get { return Field<bool>("IsNullable"); } private set { this["IsNullable"] = value; } }
		public bool IsUserDefined { get { return Field<bool>("IsUserDefined"); } private set { this["IsUserDefined"] = value; } }
		public bool IsAssemblyType { get { return Field<bool>("IsAssemblyType"); } private set { this["IsAssemblyType"] = value; } }
		public int DefaultObjectID { get { return Field<int>("DefaultObjectID"); } private set { this["DefaultObjectID"] = value; } }
		public int RuleObjectID { get { return Field<int>("RuleObjectID"); } private set { this["RuleObjectID"] = value; } }
		public bool IsTableType { get { return Field<bool>("IsTableType"); } private set { this["IsTableType"] = value; } }

		public Type()
		{
			Columns.Add(new DataColumn("Name", "sysname"));
			Columns.Add(new DataColumn("SystemTypeID", "tinyint"));
			Columns.Add(new DataColumn("UserTypeID", "int"));
			Columns.Add(new DataColumn("SchemaID", "int"));
			Columns.Add(new DataColumn("PrincipalID", "int", true));
			Columns.Add(new DataColumn("MaxLength", "smallint"));
			Columns.Add(new DataColumn("Precision", "tinyint"));
			Columns.Add(new DataColumn("Scale", "tinyint"));
			Columns.Add(new DataColumn("CollationName", "sysname", true));
			Columns.Add(new DataColumn("IsNullable", "bit", true));
			Columns.Add(new DataColumn("IsUserDefined", "bit"));
			Columns.Add(new DataColumn("IsAssemblyType", "bit"));
			Columns.Add(new DataColumn("DefaultObjectID", "int"));
			Columns.Add(new DataColumn("RuleObjectID", "int"));
			Columns.Add(new DataColumn("IsTableType", "bit"));
		}

		public override Row NewRow()
		{
			return new Type();
		}

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