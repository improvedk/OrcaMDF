using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData.Enumerations;
using OrcaMDF.Core.MetaData.SystemEntities;

namespace OrcaMDF.Core.MetaData
{
	public class DatabaseMetaData
	{
		public IList<SysAllocationUnit> SysAllocationUnits { get; internal set; }
		public IList<SysObject> SysObjects { get; internal set; }
		public IList<SysRowset> SysRowsets { get; internal set; }
		public IList<SysRowsetColumn> SysRowsetColumns { get; internal set; }

		private readonly MdfFile file;
		private readonly DataScanner scanner;

		internal DatabaseMetaData(MdfFile file)
		{
			this.file = file;
			scanner = new DataScanner(file);

			parseSysAllocationUnits();
			parseSysRowsets();
			parseSysRowsetColumns();
			parseSysObjects();
		}

		public string[] TableNames
		{
			get
			{
				return SysObjects
					.Where(x => x.Type == ObjectType.INTERNAL_TABLE || x.Type == ObjectType.SYSTEM_TABLE || x.Type == ObjectType.USER_TABLE)
					.Select(x => x.Name)
					.ToArray();
			}
		}

		public string[] UserTableNames
		{
			get
			{
				return SysObjects
					.Where(x => x.Type == ObjectType.USER_TABLE)
					.Select(x => x.Name)
					.ToArray();
			}
		}

		private void parseSysObjects()
		{
			long rowsetID = SysRowsets
								.Where(x => x.ObjectID == (int)SystemObject.sysschobjs && x.IndexID == 1)
								.Single()
								.PartitionID;

			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == rowsetID && x.Type == 1)
				.Single()
				.FirstPage;

			SysObjects = scanner.ScanLinkedPages<SysObject>(new PagePointer(pageLoc)).ToList();
		}

		private void parseSysRowsetColumns()
		{
			long rowsetID =	SysRowsets
								.Where(x => x.ObjectID == (int)SystemObject.syscolpars && x.IndexID == 1)
								.Single()
								.PartitionID;

			var pageLoc = SysAllocationUnits
				.Where(x => x.AllocationUnitID == rowsetID && x.Type == 1)
				.Single()
				.FirstPage;

			SysRowsetColumns = scanner.ScanLinkedPages<SysRowsetColumn>(new PagePointer(pageLoc)).ToList();
		}

		private void parseSysRowsets()
		{
			var pageLoc = SysAllocationUnits
							.Where(x => x.AllocationUnitID == (long) FixedSystemObjectAllocationUnits.sysrowsets)
							.Single()
							.FirstPage;

			SysRowsets = scanner.ScanLinkedPages<SysRowset>(new PagePointer(pageLoc)).ToList();
		}

		private void parseSysAllocationUnits()
		{
			var bootPage = file.GetBootPage();

			SysAllocationUnits = scanner.ScanLinkedPages<SysAllocationUnit>(bootPage.FirstSysIndexes).ToList();
		}
	}
}