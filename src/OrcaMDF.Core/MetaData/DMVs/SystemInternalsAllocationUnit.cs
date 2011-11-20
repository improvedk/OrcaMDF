using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SystemInternalsAllocationUnit : Row
	{
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

		public SystemInternalsAllocationUnit()
		{
			Columns.Add(new DataColumn("AllocationUnitID", "bigint"));
			Columns.Add(new DataColumn("Type", "tinyint"));
			Columns.Add(new DataColumn("TypeDesc", "nvarchar", true));
			Columns.Add(new DataColumn("ContainerID", "bigint"));
			Columns.Add(new DataColumn("FilegroupID", "smallint"));
			Columns.Add(new DataColumn("TotalPages", "bigint"));
			Columns.Add(new DataColumn("UsedPages", "bigint"));
			Columns.Add(new DataColumn("DataPages", "bigint"));
			Columns.Add(new DataColumn("FirstPage", "binary(6)"));
			Columns.Add(new DataColumn("RootPage", "binary(6)"));
			Columns.Add(new DataColumn("FirstIamPage", "binary(6)"));
		}

		public override Row NewRow()
		{
			return new SystemInternalsAllocationUnit();
		}

		internal static IEnumerable<SystemInternalsAllocationUnit> GetDmvData(Database db)
		{
			return db.BaseTables.sysallocunits
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
				});
		}
	}
}