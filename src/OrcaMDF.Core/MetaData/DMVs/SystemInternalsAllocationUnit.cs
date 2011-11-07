using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SystemInternalsAllocationUnit
	{
		public long AllocationUnitID { get; private set; }
		public byte Type { get; private set; }
		public string TypeDesc { get; private set; }
		public long ContainerID { get; private set; }
		public short FilegroupID { get; private set; }
		public long TotalPages { get { throw new NotImplementedException(); } } // TODO: Get from OpenRowset(TABLE ALUCOUNT, au.ownerid, au.type) ct  
		public long UsedPages { get { throw new NotImplementedException(); } } // TODO: Get from OpenRowset(TABLE ALUCOUNT, au.ownerid, au.type) ct  
		public long DataPages { get { throw new NotImplementedException(); } } // TODO: Get from OpenRowset(TABLE ALUCOUNT, au.ownerid, au.type) ct  
		public PagePointer FirstPage { get; private set; }
		public PagePointer RootPage { get; private set; }
		public PagePointer FirstIamPage { get; private set; }

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