using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SqlModule : Row
	{
		private const string CACHE_KEY = "DMV_SqlModule";

		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("ObjectID", "int"),
		        new DataColumn("Definition", "nvarchar(MAX)", true),
		        new DataColumn("UsesAnsiNulls", "bit", true),
		        new DataColumn("UsesQuotedIdentifier", "bit", true),
		        new DataColumn("IsSchemaBound", "bit", true),
		        new DataColumn("UsesDatabaseCollation", "bit", true),
		        new DataColumn("IsRecompiled", "bit", true),
		        new DataColumn("NullOnNullInput", "bit", true),
		        new DataColumn("ExecuteAsPrincipalID", "int", true)
		    });

		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public string Definition { get { return Field<string>("Definition"); } private set { this["Definition"] = value; } }
		public bool? UsesAnsiNulls { get { return Field<bool?>("UsesAnsiNulls"); } private set { this["UsesAnsiNulls"] = value; } }
		public bool? UsesQuotedIdentifier { get { return Field<bool?>("UsesQuotedIdentifier"); } private set { this["UsesQuotedIdentifier"] = value; } }
		public bool? IsSchemaBound { get { return Field<bool?>("IsSchemaBound"); } private set { this["IsSchemaBound"] = value; } }
		public bool? UsesDatabaseCollation { get { return Field<bool?>("UsesDatabaseCollation"); } private set { this["UsesDatabaseCollation"] = value; } }
		public bool? IsRecompiled { get { return Field<bool?>("IsRecompiled"); } private set { this["IsRecompiled"] = value; } }
		public bool? NullOnNullInput { get { return Field<bool?>("NullOnNullInput"); } private set { this["NullOnNullInput"] = value; } }
		public int? ExecuteAsPrincipalID { get { return Field<int?>("ExecuteAsPrincipalID"); } private set { this["ExecuteAsPrincipalID"] = value; } }

		public SqlModule()
			: base(schema)
		{ }

		public override Row NewRow()
		{
			return new Table();
		}

		internal static IEnumerable<SqlModule> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				var types = new [] { "TR", "P", "V", "FN", "IF", "TF", "RF", "IS", "R", "D" };

				db.ObjectCache[CACHE_KEY] = db.BaseTables.sysschobjs
					.Where(o => o.pclass != 100 && types.Contains(o.type.Trim()))
					.Select(o => new SqlModule
					    {
					        ObjectID = o.id,
					        Definition = db.BaseTables.sysobjvalues
								.Where(v => v.objid == o.id)
								.Select(v => Encoding.ASCII.GetString(v.imageval))
								.FirstOrDefault(),
					        UsesAnsiNulls = Convert.ToBoolean(o.status & 0x40000),
					        UsesQuotedIdentifier = Convert.ToBoolean(o.status & 0x80000),
					        IsSchemaBound = Convert.ToBoolean(o.status & 0x20000),
					        UsesDatabaseCollation = Convert.ToBoolean(o.status & 0x100000),
					        IsRecompiled = Convert.ToBoolean(o.status & 0x400000),
					        NullOnNullInput = Convert.ToBoolean(o.status & 0x200000),
					        ExecuteAsPrincipalID =
					            db.BaseTables.syssingleobjrefs.Where(x => x.depid == o.id && x.@class == 22 && x.depsubid == 0).
					            Select(x => x.indepid).FirstOrDefault()
					    })
					.ToList();
			}

			return (IEnumerable<SqlModule>)db.ObjectCache[CACHE_KEY];
		}
	}
}