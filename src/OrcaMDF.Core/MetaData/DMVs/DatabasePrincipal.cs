using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class DatabasePrincipal : Row
	{
		private const string CACHE_KEY = "DMV_DatabasePrincipal";

		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("Name", "sysname"),
		        new DataColumn("PrincipalID", "int"),
		        new DataColumn("Type", "char(1)"),
		        new DataColumn("TypeDesc", "nvarchar", true),
		        new DataColumn("DefaultSchemaName", "sysname", true),
		        new DataColumn("CreateDate", "datetime"),
		        new DataColumn("ModifyDate", "datetime"),
		        new DataColumn("OwningPrincipalID", "int", true),
		        new DataColumn("Sid", "varbinary", true),
		        new DataColumn("IsFixedRole", "bit")
		    });

		public string Name { get { return Field<string>("Name"); } private set { this["Name"] = value; } }
		public int PrincipalID { get { return Field<int>("PrincipalID"); } private set { this["PrincipalID"] = value; } }
		public string Type { get { return Field<string>("Type"); } private set { this["Type"] = value; } }
		public string TypeDesc { get { return Field<string>("TypeDesc"); } private set { this["TypeDesc"] = value; } }
		public string DefaultSchemaName { get { return Field<string>("DefaultSchemaName"); } private set { this["DefaultSchemaName"] = value; } }
		public DateTime CreateDate { get { return Field<DateTime>("CreateDate"); } private set { this["CreateDate"] = value; } }
		public DateTime ModifyDate { get { return Field<DateTime>("ModifyDate"); } private set { this["ModifyDate"] = value; } }
		public int? OwningPrincipalID { get { return Field<int?>("OwningPrincipalID"); } private set { this["OwningPrincipalID"] = value; } }
		public byte[] Sid { get { return Field<byte[]>("Sid"); } private set { this["Sid"] = value; } }
		public bool IsFixedRole { get { return Field<bool>("IsFixedRole"); } private set { this["IsFixedRole"] = value; } }

		public DatabasePrincipal()
			: base(schema)
		{ }

		public override Row NewRow()
		{
			return new Table();
		}

		internal static IEnumerable<DatabasePrincipal> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.BaseTables.sysowners
					.Where(u => u.type.Trim() != "L")
					.Select(u => new DatabasePrincipal
					    {
					        Name = u.name,
							PrincipalID = u.id,
							Type = u.type,
							TypeDesc = db.BaseTables.syspalnames
								.Where(n => n.@class == "USTY" && n.value == u.type)
								.Select(n => n.name)
								.FirstOrDefault(),
							DefaultSchemaName = u.dfltsch,
							CreateDate = u.created,
							ModifyDate = u.modified,
							OwningPrincipalID = db.BaseTables.syssingleobjrefs
								.Where(r => r.depid == u.id && r.@class == 51 && r.depsubid == 0)
								.Select(r => r.indepid)
								.FirstOrDefault(),
							Sid = u.sid,
							IsFixedRole = u.id >= 16384 && u.id < 16400
					    })
					.ToList();
			}

			return (IEnumerable<DatabasePrincipal>)db.ObjectCache[CACHE_KEY];
		}
	}
}