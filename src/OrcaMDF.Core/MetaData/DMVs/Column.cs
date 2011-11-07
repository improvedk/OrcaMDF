using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Column
	{
		public int ObjectID { get; private set; }
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