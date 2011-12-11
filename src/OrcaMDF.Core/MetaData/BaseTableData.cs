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
		internal IList<sysrscol> sysrscols { get; private set; }
		internal IList<syssingleobjref> syssingleobjrefs { get; private set; }
		
		private IList<sysidxstat> _sysidxstats;
		internal IList<sysidxstat> sysidxstats
		{
			get { return _sysidxstats ?? (_sysidxstats = scanner.ScanTable<sysidxstat>("sysidxstats").ToList()); }
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

			// These are the very core base tables that we'll need to dynamically construct the schema of any other
			// required tables. By aggresively parsing these, we can do lazy loading of the rest.
			parseSysallocunits();
			parseSysrowsets();
			parseSyscolpars();
			parseSysobjects();
			parseSysscalartypes();
			parseSysrscols();
			parseSyssingleobjrefs();
		}

		private void parseSyssingleobjrefs()
		{
			// Using a fixed object ID, we can look up the partition for sysscalartypes and scan the hobt AU from there
			long rowsetID = sysrowsets
				.Where(x => x.idmajor == (int)SystemObject.syssingleobjrefs && x.idminor == 1)
				.Single()
				.rowsetid;

			var pageLoc = new PagePointer(
				sysallocunits
					.Where(x => x.auid == rowsetID && x.type == 1)
					.Single()
					.pgfirst
			);

			syssingleobjrefs = scanner.ScanLinkedDataPages<syssingleobjref>(pageLoc, CompressionContext.None).ToList();
		}

		private void parseSysrscols()
		{
			// Using a fixed object ID, we can look up the partition for sysscalartypes and scan the hobt AU from there
			long rowsetID = sysrowsets
				.Where(x => x.idmajor == (int)SystemObject.sysrscols && x.idminor == 1)
				.Single()
				.rowsetid;

			var pageLoc = new PagePointer(
				sysallocunits
					.Where(x => x.auid == rowsetID && x.type == 1)
					.Single()
					.pgfirst
			);

			sysrscols = scanner.ScanLinkedDataPages<sysrscol>(pageLoc, CompressionContext.None).ToList();
		}

		private void parseSysscalartypes()
		{
			// Using a fixed object ID, we can look up the partition for sysscalartypes and scan the hobt AU from there
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
			
			sysscalartypes = scanner.ScanLinkedDataPages<sysscalartype>(pageLoc, CompressionContext.None).ToList();
		}

		private void parseSysobjects()
		{
			// Using a fixed object ID, we can look up the partition for sysschobjs and scan the hobt AU from there
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

			sysschobjs = scanner.ScanLinkedDataPages<sysschobj>(pageLoc, CompressionContext.None).ToList();
		}

		private void parseSyscolpars()
		{
			// Using a fixed object ID, we can look up the partition for syscolpars and scan the hobt AU from there
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

			syscolpars = scanner.ScanLinkedDataPages<syscolpar>(pageLoc, CompressionContext.None).ToList();
		}

		private void parseSysrowsets()
		{
			// Using a fixed allocation unit ID, we can look up the hobt AU and scan it
			var pageLoc = new PagePointer(
				sysallocunits
			        .Where(x => x.auid == FixedSystemObjectAllocationUnits.sysrowsets)
			        .Single()
			        .pgfirst
			);

			sysrowsets = scanner.ScanLinkedDataPages<sysrowset>(pageLoc, CompressionContext.None).ToList();
		}

		private void parseSysallocunits()
		{
			// Though this has a fixed first-page location at (1:16) we'll read it from the boot page to be sure
			var bootPage = db.GetBootPage();
			sysallocunits = scanner.ScanLinkedDataPages<sysallocunit>(bootPage.FirstSysIndexes, CompressionContext.None).ToList();
		}
	}
}