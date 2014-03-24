using NUnit.Framework;
using OrcaMDF.Framework;
using OrcaMDF.RawCore.Utilities.SQL2012;
using System;
using System.Linq;

namespace OrcaMDF.RawCore.Tests.Utilities.SQL2012
{
	public class SQL2012SysrowsetsTests : BaseFixture
	{
		[Test]
		public void sysrowsets()
		{
			var db = new RawDataFile(AW2012Path);
			var pages = db.Pages.Where(x => x.Header.ObjectID == SQL2012Sysrowsets.ObjectID && x.Header.Type == PageType.Data);
			var rows = RawColumnParser.Parse(pages, SQL2012Sysrowsets.Schema).Select(SQL2012Sysrowsets.Row).ToList();

			// Aggregates
			Assert.AreEqual(158, rows.Count);
			Assert.AreEqual(72057594041729024, rows.Max(x => x.rowsetid));
			Assert.AreEqual(196608, rows.Min(x => x.rowsetid));
			Assert.AreEqual(20827366792640280, (long)rows.Average(x => x.rowsetid));
			Assert.AreEqual(7662, (long)rows.Average(x => x.status));
			Assert.AreEqual(2266, (long)rows.Average(x => x.maxleaf));

			// Individual rows
			var testRow = rows.Single(x => x.rowsetid == 562949954535424);
			Assert.AreEqual(70, testRow.status);
			Assert.AreEqual(534, testRow.maxint);

			testRow = rows.Single(x => x.rowsetid == 72057594039697408);
			Assert.AreEqual(117575457, testRow.idmajor);
			Assert.AreEqual(1453, testRow.maxleaf);
		}
	}
}