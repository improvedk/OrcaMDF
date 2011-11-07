using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData.BaseTables;
using OrcaMDF.Core.MetaData.Enumerations;

namespace OrcaMDF.Core.MetaData
{
	internal class BaseTableData
	{
		private readonly Database db;
		private readonly DataScanner scanner;

		// These are crucial base tables that are eagerly scanned on instantiation
		internal IList<sysallocunit> sysallocunits { get; private set; }
		internal IList<syscolpar> syscolpars { get; private set; }
		internal IList<sysschobj> sysschobjs { get; private set; }
		internal IList<sysscalartype> sysscalartypes { get; private set; }
		internal IList<sysrowset> sysrowsets { get; private set; }

		private IList<sysrscol> _sysrscols;
		public IList<sysrscol> sysrscols
		{
			get { return _sysrscols ?? (_sysrscols = scanner.ScanTable<sysrscol>("sysrscols").ToList()); }
		}

		private IList<sysidxstat> _sysidxstats;
		internal IList<sysidxstat> sysidxstats
		{
			get { return _sysidxstats ?? (_sysidxstats = scanner.ScanTable<sysidxstat>("sysidxstats").ToList()); }
		}
		
		private IList<syssingleobjref> _syssingleobjrefs;
		internal IList<syssingleobjref> syssingleobjrefs
		{
			get { return _syssingleobjrefs ?? (_syssingleobjrefs = scanner.ScanTable<syssingleobjref>("syssingleobjrefs").ToList()); }
		}

		private IList<syspalvalue> _syspalvalues;
		internal IList<syspalvalue> syspalvalues
		{
			get { return _syspalvalues ?? (_syspalvalues = syspalvalue.GetServer2008R2HardcodedValues()); }
		}

		private IList<syspalname> _syspalnames;
		internal IList<syspalname> syspalnames
		{
			get { return _syspalnames ?? (_syspalnames = syspalname.GetServer2008R2HardcodedValues()); }
		}

		private IList<sysiscol> _sysiscols;
		internal IList<sysiscol> sysiscols
		{
			get { return _sysiscols ?? (_sysiscols = scanner.ScanTable<sysiscol>("sysiscols").ToList()); }
		}

		internal BaseTableData(Database db)
		{
			this.db = db;
			scanner = new DataScanner(db);

			parseSysallocunits();
			parseSysrowsets();
			parseSyscolpars();
			parseSysobjects();
			parseSysscalartypes();
		}

		private void parseSysscalartypes()
		{
			long rowsetID = sysrowsets
				.Where(x => x.idmajor == (int)SystemObject.sysscalartypes && x.idminor == 1)
				.Single()
				.rowsetid;

			var pageLoc = new PagePointer(
				sysallocunits
					.Where(x => x.auid == rowsetID && x.type == 1)
					.Single()
					.pgfirst
			);

			sysscalartypes = scanner.ScanLinkedDataPages<sysscalartype>(pageLoc).ToList();
		}

		private void parseSysobjects()
		{
			long rowsetID = sysrowsets
				.Where(x => x.idmajor == (int)SystemObject.sysschobjs && x.idminor == 1)
				.Single()
				.rowsetid;

			var pageLoc = new PagePointer(
				sysallocunits
					.Where(x => x.auid == rowsetID && x.type == 1)
					.Single()
					.pgfirst
			);

			sysschobjs = scanner.ScanLinkedDataPages<sysschobj>(pageLoc).ToList();
		}

		private void parseSyscolpars()
		{
			long rowsetID = sysrowsets
				.Where(x => x.idmajor == (int)SystemObject.syscolpars && x.idminor == 1)
				.Single()
				.rowsetid;

			var pageLoc = new PagePointer(
				sysallocunits
					.Where(x => x.auid == rowsetID && x.type == 1)
					.Single()
					.pgfirst
			);

			syscolpars = scanner.ScanLinkedDataPages<syscolpar>(pageLoc).ToList();
		}

		private void parseSysrowsets()
		{
			var pageLoc = new PagePointer(
				sysallocunits
			        .Where(x => x.auid == FixedSystemObjectAllocationUnits.sysrowsets)
			        .Single()
			        .pgfirst
			);

			sysrowsets = scanner.ScanLinkedDataPages<sysrowset>(pageLoc).ToList();
		}

		private void parseSysallocunits()
		{
			// Though this has a fixed first-page location at (1:16) we'll read it from the boot page to be sure
			var bootPage = db.GetBootPage();
			sysallocunits = scanner.ScanLinkedDataPages<sysallocunit>(bootPage.FirstSysIndexes).ToList();
		}
	}
}