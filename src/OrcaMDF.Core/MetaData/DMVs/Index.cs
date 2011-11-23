using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Index : Row
	{
		private const string CACHE_KEY = "DMV_Index";

		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("ObjectID", "int"),
				new DataColumn("Name", "sysname", true),
				new DataColumn("IndexID", "int"),
				new DataColumn("Type", "tinyint"),
				new DataColumn("TypeDesc", "nvarchar", true),
				new DataColumn("IsUnique", "bit", true),
				new DataColumn("DataSpaceID", "int"),
				new DataColumn("IgnoreDupKey", "bit", true),
				new DataColumn("IsPrimaryKey", "bit", true),
				new DataColumn("IsUniqueConstraint", "bit", true),
				new DataColumn("FillFactor", "tinyint"),
				new DataColumn("IsPadded", "bit", true),
				new DataColumn("IsDisabled", "bit", true),
				new DataColumn("IsHypothetical", "bit", true),
				new DataColumn("AllowRowLocks", "bit", true),
				new DataColumn("AllowPageLocks", "bit", true),
				new DataColumn("HasFilter", "bit", true),
				new DataColumn("FilterDefinition", "nvarchar", true)
		    });

		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public string Name { get { return Field<string>("Name"); } private set { this["Name"] = value; } }
		public int IndexID { get { return Field<int>("IndexID"); } private set { this["IndexID"] = value; } }
		public byte Type { get { return Field<byte>("Type"); } private set { this["Type"] = value; } }
		public string TypeDesc { get { return Field<string>("TypeDesc"); } private set { this["TypeDesc"] = value; } }
		public int DataSpaceID { get { return Field<int>("DataSpaceID"); } private set { this["DataSpaceID"] = value; } }
		public bool? IgnoreDupKey { get { return Field<bool?>("IgnoreDupKey"); } private set { this["IgnoreDupKey"] = value; } }
		public byte FillFactor { get { return Field<byte>("FillFactor"); } private set { this["FillFactor"] = value; } }
		public bool IsUnique { get { return Field<bool>("IsUnique"); } private set { this["IsUnique"] = value; } }
		public bool IsPrimaryKey { get { return Field<bool>("IsPrimaryKey"); } private set { this["IsPrimaryKey"] = value; } }
		public bool IsUniqueConstraint { get { return Field<bool>("IsUniqueConstraint"); } private set { this["IsUniqueConstraint"] = value; } }
		public bool IsPadded { get { return Field<bool>("IsPadded"); } private set { this["IsPadded"] = value; } }
		public bool IsDisabled { get { return Field<bool>("IsDisabled"); } private set { this["IsDisabled"] = value; } }
		public bool IsHypothetical { get { return Field<bool>("IsHypothetical"); } private set { this["IsHypothetical"] = value; } }
		public bool AllowRowLocks { get { return Field<bool>("AllowRowLocks"); } private set { this["AllowRowLocks"] = value; } }
		public bool AllowPageLocks { get { return Field<bool>("AllowPageLocks"); } private set { this["AllowPageLocks"] = value; } }
		public bool HasFilter { get { return Field<bool>("HasFilter"); } private set { this["HasFilter"] = value; } }
		public string FilterDefinition { get { return Field<string>("FilterDefinition"); } private set { this["FilterDefinition"] = value; } } // TODO

		public Index() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new Index();
		}

		internal static IEnumerable<Index> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.BaseTables.sysidxstats
					.Where(i => (i.status & 1) != 0)
					.Select(i => new Index
			       		{
			       			ObjectID = i.id,
			       			Name = i.name,
			       			IndexID = i.indid,
			       			Type = i.type,
			       			TypeDesc = db.BaseTables.syspalvalues
			       				.Where(n => n.@class == "IDXT" && n.value == i.type)
			       				.Select(n => n.name)
			       				.Single(),
			       			DataSpaceID = i.dataspace,
			       			FillFactor = i.fillfact,
			       			IsUnique = Convert.ToBoolean(i.status & 0x8),
			       			IsPrimaryKey = Convert.ToBoolean(i.status & 0x20),
			       			IsUniqueConstraint = Convert.ToBoolean(i.status & 0x40),
			       			IsPadded = Convert.ToBoolean(i.status & 0x10),
			       			IsDisabled = Convert.ToBoolean(i.status & 0x80),
			       			IsHypothetical = Convert.ToBoolean(i.status & 0x100),
			       			AllowRowLocks = Convert.ToBoolean(1 - (i.status & 512) / 512),
			       			AllowPageLocks = Convert.ToBoolean(1 - (i.status & 1024) / 1024),
			       			HasFilter = Convert.ToBoolean(i.status & 0x20000)
			       		})
					.ToList();
			}

			return (IEnumerable<Index>)db.ObjectCache[CACHE_KEY];
		}
	}
}