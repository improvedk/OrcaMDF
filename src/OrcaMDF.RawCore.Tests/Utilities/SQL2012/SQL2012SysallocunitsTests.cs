using NUnit.Framework;
using OrcaMDF.Framework;
using OrcaMDF.RawCore.Utilities.SQL2012;
using System.Linq;

namespace OrcaMDF.RawCore.Tests.Utilities.SQL2012
{
	public class SQL2012SysallocunitsTests : BaseFixture
	{
		[Test]
		public void sysallocunits()
		{
			var db = new RawDataFile(AW2012Path);
			var pages = db.Pages.Where(x => x.Header.ObjectID == SQL2012Sysallocunits.ObjectID && x.Header.Type == PageType.Data);
			var rows = RawColumnParser.Parse(pages, SQL2012Sysallocunits.Schema).Select(SQL2012Sysallocunits.Row).ToList();

			// Aggregates
			Assert.AreEqual(181, rows.Count);
			Assert.AreEqual(27335730062154916, (long)rows.Average(x => x.auid));
			Assert.AreEqual(3, (int)rows.Average(x => x.pcdata));

			// Individual rows
			var testRow = rows.Single(x => x.auid == 562949958533120);
			Assert.AreEqual(562949958533120, testRow.ownerid);
			Assert.AreEqual(0, testRow.pcdata);

			testRow = rows.Single(x => x.auid == 196608);
			Assert.AreEqual(1, testRow.type);
			Assert.AreEqual(17, testRow.pcreserved);
			Assert.AreEqual(13, testRow.pcused);
			Assert.AreEqual(new byte[] { 0x57, 0x00, 0x00, 0x00, 0x01, 0x00 }, testRow.pgroot);
		}
	}
}