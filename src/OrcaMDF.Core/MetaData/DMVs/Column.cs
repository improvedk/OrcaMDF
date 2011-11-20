using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Column : Row
	{
		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public string Name { get { return Field<string>("Name"); } private set { this["Name"] = value; } }
		public int ColumnID { get { return Field<int>("ColumnID"); } private set { this["ColumnID"] = value; } }
		public byte SystemTypeID { get { return Field<byte>("SystemTypeID"); } private set { this["SystemTypeID"] = value; } }
		public int UserTypeID { get { return Field<int>("UserTypeID"); } private set { this["UserTypeID"] = value; } }
		public short MaxLength { get { return Field<short>("MaxLength"); } private set { this["MaxLength"] = value; } }
		public byte Precision { get { return Field<byte>("Precision"); } private set { this["Precision"] = value; } }
		public byte Scale { get { return Field<byte>("Scale"); } private set { this["Scale"] = value; } }
		public string CollationName { get { return Field<string>("CollationName"); } private set { this["CollationName"] = value; } } // TODO
		public bool IsNullable { get { return Field<bool>("IsNullable"); } private set { this["IsNullable"] = value; } }
		public bool IsAnsiPadded { get { return Field<bool>("IsAnsiPadded"); } private set { this["IsAnsiPadded"] = value; } }
		public bool IsRowGuidCol { get { return Field<bool>("IsRowGuidCol"); } private set { this["IsRowGuidCol"] = value; } }
		public bool IsIdentity { get { return Field<bool>("IsIdentity"); } private set { this["IsIdentity"] = value; } }
		public bool IsComputed { get { return Field<bool>("IsComputed"); } private set { this["IsComputed"] = value; } }
		public bool IsFilestream { get { return Field<bool>("IsFilestream"); } private set { this["IsFilestream"] = value; } }
		public bool IsReplicated { get { return Field<bool>("IsReplicated"); } private set { this["IsReplicated"] = value; } }
		public bool IsNonSqlSubscribed { get { return Field<bool>("IsNonSqlSubscribed"); } private set { this["IsNonSqlSubscribed"] = value; } }
		public bool IsMergePublished { get { return Field<bool>("IsMergePublished"); } private set { this["IsMergePublished"] = value; } }
		public bool IsDtsReplicated { get { return Field<bool>("IsDtsReplicated"); } private set { this["IsDtsReplicated"] = value; } }
		public bool IsXmlDocument { get { return Field<bool>("IsXmlDocument"); } private set { this["IsXmlDocument"] = value; } }
		public bool IsSparse { get { return Field<bool>("IsSparse"); } private set { this["IsSparse"] = value; } }
		public bool IsColumnSet { get { return Field<bool>("IsColumnSet"); } private set { this["IsColumnSet"] = value; } }
		public int XmlCollectionID { get { return Field<int>("XmlCollectionID"); } private set { this["XmlCollectionID"] = value; } }
		public int DefaultObjectID { get { return Field<int>("DefaultObjectID"); } private set { this["DefaultObjectID"] = value; } }
		public int RuleObjectID { get { return Field<int>("RuleObjectID"); } private set { this["RuleObjectID"] = value; } }

		public Column()
		{
			Columns.Add(new DataColumn("ObjectID", "int"));
			Columns.Add(new DataColumn("Name", "sysname", true));
			Columns.Add(new DataColumn("ColumnID", "int"));
			Columns.Add(new DataColumn("SystemTypeID", "tinyint"));
			Columns.Add(new DataColumn("UserTypeID", "int"));
			Columns.Add(new DataColumn("MaxLength", "smallint"));
			Columns.Add(new DataColumn("Precision", "tinyint"));
			Columns.Add(new DataColumn("Scale", "tinyint"));
			Columns.Add(new DataColumn("CollationName", "sysname", true));
			Columns.Add(new DataColumn("IsNullable", "bit", true));
			Columns.Add(new DataColumn("IsAnsiPadded", "bit"));
			Columns.Add(new DataColumn("IsRowGuidCol", "bit"));
			Columns.Add(new DataColumn("IsIdentity", "bit"));
			Columns.Add(new DataColumn("IsComputed", "bit"));
			Columns.Add(new DataColumn("IsFilestream", "bit"));
			Columns.Add(new DataColumn("IsReplicated", "bit", true));
			Columns.Add(new DataColumn("IsNonSqlSubscribed", "bit", true));
			Columns.Add(new DataColumn("IsMergePublished", "bit", true));
			Columns.Add(new DataColumn("IsDtsReplicated", "bit", true));
			Columns.Add(new DataColumn("IsXmlDocument", "bit"));
			Columns.Add(new DataColumn("XmlCollectionID", "int"));
			Columns.Add(new DataColumn("DefaultObjectID", "int"));
			Columns.Add(new DataColumn("RuleObjectID", "int"));
			Columns.Add(new DataColumn("IsSparse", "bit", true));
			Columns.Add(new DataColumn("IsColumnSet", "bit", true));
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