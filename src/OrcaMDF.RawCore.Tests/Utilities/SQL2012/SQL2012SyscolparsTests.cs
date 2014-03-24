using NUnit.Framework;
using OrcaMDF.Framework;
using OrcaMDF.RawCore.Utilities.SQL2012;
using System.Linq;

namespace OrcaMDF.RawCore.Tests.Utilities.SQL2012
{
	public class SQL2012SyscolparsTests : BaseFixture
	{
		[Test]
		public void syscolpars()
		{
			var db = new RawDataFile(AW2012Path);
			var pages = db.Pages.Where(x => x.Header.ObjectID == SQL2012Syscolpars.ObjectID && x.Header.Type == PageType.Data);
			var rows = RawColumnParser.Parse(pages, SQL2012Syscolpars.Schema).Select(SQL2012Syscolpars.Row).ToList();

			// Aggregates
			Assert.AreEqual(860, rows.Count);
			Assert.AreEqual(317769461, (int)rows.Average(x => x.id));
			Assert.AreEqual(105, (int)rows.Average(x => x.xtype));

			var nullRows = rows.Where(x => x.name == null).ToList();

			Assert.AreEqual(8, (int)rows.Average(x => x.name.Length));

			// Individual rows
			var testRow = rows.Single(x => x.id == 25 && x.colid == 12);
			Assert.AreEqual("bXVTDocidUseBaseT", testRow.name);
			Assert.AreEqual(48, testRow.utype);

			testRow = rows.Single(x => x.id == 73 && x.colid == 13);
			Assert.AreEqual("enddlgseq", testRow.name);
			Assert.AreEqual(127, testRow.utype);
			Assert.AreEqual(null, testRow.idtval);
		}
	}
}