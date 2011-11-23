using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SystemInternalsAllocationUnit : Row
	{
		private const string CACHE_KEY = "DMV_SystemInternalsAllocationUnit";

		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("AllocationUnitID", "bigint"),
				new DataColumn("Type", "tinyint"),
				new DataColumn("TypeDesc", "nvarchar", true),
				new DataColumn("ContainerID", "bigint"),
				new DataColumn("FilegroupID", "smallint"),
				new DataColumn("TotalPages", "bigint"),
				new DataColumn("UsedPages", "bigint"),
				new DataColumn("DataPages", "bigint"),
				new DataColumn("FirstPage", "binary(6)"),
				new DataColumn("RootPage", "binary(6)"),
				new DataColumn("FirstIamPage", "binary(6)")
		    });

		public long AllocationUnitID { get { return Field<long>("AllocationUnitID"); } private set { this["AllocationUnitID"] = value; } }
		public byte Type { get { return Field<byte>("Type"); } private set { this["Type"] = value; } }
		public string TypeDesc { get { return Field<string>("TypeDesc"); } private set { this["TypeDesc"] = value; } }
		public long ContainerID { get { return Field<long>("ContainerID"); } private set { this["ContainerID"] = value; } }
		public short FilegroupID { get { return Field<short>("FilegroupID"); } private set { this["FilegroupID"] = value; } }
		public long? TotalPages { get { return Field<long?>("TotalPages"); } private set { this["TotalPages"] = value; } } // TODO
		public long? UsedPages { get { return Field<long?>("UsedPages"); } private set { this["UsedPages"] = value; } } // TODO
		public long? DataPages { get { return Field<long?>("DataPages"); } private set { this["DataPages"] = value; } } // TODO
		public PagePointer FirstPage { get { return Field<PagePointer>("FirstPage"); } private set { this["FirstPage"] = value; } }
		public PagePointer RootPage { get { return Field<PagePointer>("RootPage"); } private set { this["RootPage"] = value; } }
		public PagePointer FirstIamPage { get { return Field<PagePointer>("FirstIamPage"); } private set { this["FirstIamPage"] = value; } }

		public SystemInternalsAllocationUnit() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new SystemInternalsAllocationUnit();
		}

		internal static IEnumerable<SystemInternalsAllocationUnit> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.BaseTables.sysallocunits
					.Select(au => new SystemInternalsAllocationUnit
						{
							AllocationUnitID = au.auid,
							Type = au.type,
							TypeDesc = db.BaseTables.syspalvalues
								.Where(ip => ip.@class == "AUTY" && ip.value == au.type)
								.Select(n => n.name)
								.Single(),
							ContainerID = au.ownerid,
							FilegroupID = au.fgid,
							FirstPage = new PagePointer(au.pgfirst),
							RootPage = new PagePointer(au.pgroot),
							FirstIamPage = new PagePointer(au.pgfirstiam)
						})
					.ToList();
			}

			return (IEnumerable<SystemInternalsAllocationUnit>)db.ObjectCache[CACHE_KEY];
		}
	}
}