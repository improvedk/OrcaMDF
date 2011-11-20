using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Column : Row
	{
		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public string Name { get; private set; }
		public int ColumnID { get; private set; }
		public byte SystemTypeID { get; private set; }
		public int UserTypeID { get; private set; }
		public short MaxLength { get; private set; }
		public byte Precision { get; private set; }
		public byte Scale { get; private set; }
		public string CollationName { get { throw new NotImplementedException(); } } // TODO: Get value from CollationPropertyFromId() function
		public bool IsNullable { get; private set; }
		public bool IsAnsiPadded { get; private set; }
		public bool IsRowGuidCol { get; private set; }
		public bool IsIdentity { get; private set; }
		public bool IsComputed { get; private set; }
		public bool IsFilestream { get; private set; }
		public bool IsReplicated { get; private set; }
		public bool IsNonSqlSubscribed { get; private set; }
		public bool IsMergePublished { get; private set; }
		public bool IsDtsReplicated { get; private set; }
		public bool IsXmlDocument { get; private set; }
		public bool IsSparse { get; private set; }
		public bool IsColumnSet { get; private set; }
		public int XmlCollectionID { get; private set; }
		public int DefaultObjectID { get; private set; }
		public int RuleObjectID { get; private set; }

		public Column()
		{
			Columns.Add(new DataColumn("object_id", "int"));
			Columns.Add(new DataColumn("name", "sysname", true));
			Columns.Add(new DataColumn("column_id", "int"));
			Columns.Add(new DataColumn("system_type_id", "tinyint"));
			Columns.Add(new DataColumn("user_type_id", "int"));
			Columns.Add(new DataColumn("max_length", "smallint"));
			Columns.Add(new DataColumn("precision", "tinyint"));
			Columns.Add(new DataColumn("scale", "tinyint"));
			Columns.Add(new DataColumn("collation_name", "sysname", true));
			Columns.Add(new DataColumn("is_nullable", "bit", true));
			Columns.Add(new DataColumn("is_ansi_padded", "bit"));
			Columns.Add(new DataColumn("is_rowguidcol", "bit"));
			Columns.Add(new DataColumn("is_identity", "bit"));
			Columns.Add(new DataColumn("is_computed", "bit"));
			Columns.Add(new DataColumn("is_filestream", "bit"));
			Columns.Add(new DataColumn("is_replicated", "bit", true));
			Columns.Add(new DataColumn("is_non_sql_subscribed", "bit", true));
			Columns.Add(new DataColumn("is_merge_published", "bit", true));
			Columns.Add(new DataColumn("is_dts_replicated", "bit", true));
			Columns.Add(new DataColumn("is_xml_document", "bit"));
			Columns.Add(new DataColumn("xml_collection_id", "int"));
			Columns.Add(new DataColumn("default_object_id", "int"));
			Columns.Add(new DataColumn("rule_object_id", "int"));
			Columns.Add(new DataColumn("is_sparse", "bit", true));
			Columns.Add(new DataColumn("is_column_set", "bit", true));
		}

		public override Row NewRow()
		{
			return new Column();
		}

		internal static IEnumerable<Column> GetDmvData(Database db)
		{
			return db.BaseTables.syscolpars
				.Where(c => c.number == 0)
				.Select(c => new Column
				{
					ObjectID = c.id,
					Name = c.name,
					ColumnID = c.colid,
					SystemTypeID = c.xtype,
					UserTypeID = c.utype,
					MaxLength = c.length,
					Precision = c.prec,
					Scale = c.scale,
					XmlCollectionID = c.xmlns,
					DefaultObjectID = c.dflt,
					RuleObjectID = c.chk,
					IsNullable = Convert.ToBoolean(1 - (c.status & 1)),
					IsAnsiPadded = Convert.ToBoolean(c.status & 2),
					IsRowGuidCol = Convert.ToBoolean(c.status & 8),
					IsIdentity = Convert.ToBoolean(c.status & 4),
					IsComputed = Convert.ToBoolean(c.status & 16),
					IsFilestream = Convert.ToBoolean(c.status & 32),
					IsReplicated = Convert.ToBoolean(c.status & 0x020000),
					IsNonSqlSubscribed = Convert.ToBoolean(c.status & 0x040000),
					IsMergePublished = Convert.ToBoolean(c.status & 0x080000),
					IsDtsReplicated = Convert.ToBoolean(c.status & 0x100000),
					IsXmlDocument = Convert.ToBoolean(c.status & 2048),
					IsSparse = Convert.ToBoolean(c.status & 0x1000000),
					IsColumnSet = Convert.ToBoolean(c.status & 0x2000000)
				});
		}
	}
}