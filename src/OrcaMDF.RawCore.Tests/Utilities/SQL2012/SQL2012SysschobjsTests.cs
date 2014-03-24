using NUnit.Framework;
using OrcaMDF.Framework;
using OrcaMDF.RawCore.Utilities.SQL2012;
using System;
using System.Linq;

namespace OrcaMDF.RawCore.Tests.Utilities.SQL2012
{
	public class SQL2012SysschobjsTests : BaseFixture
	{
		[Test]
		public void sysschobjs()
		{
			var db = new RawDataFile(AW2012Path);
			var pages = db.Pages.Where(x => x.Header.ObjectID == SQL2012Sysschobjs.ObjectID && x.Header.Type == PageType.Data);
			var rows = RawColumnParser.Parse(pages, SQL2012Sysschobjs.Schema).Select(SQL2012Sysschobjs.Row).ToList();

			// Aggregates
			Assert.AreEqual(2291, rows.Count);
			Assert.AreEqual(48296, (int)rows.Average(x => x.status));
			Assert.AreEqual(-394768470, (long)rows.Average(x => x.id));

			// Individual rows
			var testRow = rows.Single(x => x.id == -93184835);
			Assert.AreEqual("sp_helpsrvrolemember", testRow.name);
			Assert.AreEqual(Convert.ToDateTime("2011-11-04 21:12:23.257"), testRow.created);

			testRow = rows.Single(x => x.id == 60);
			Assert.AreEqual("sysobjvalues", testRow.name);
			Assert.AreEqual(Convert.ToDateTime("2008-07-09 16:20:00.633"), testRow.modified);

			testRow = rows.Single(x => x.id == 1977058079);
			Assert.AreEqual("QueryNotificationErrorsQueue", testRow.name);
			Assert.AreEqual(Convert.ToDateTime("2008-07-09 16:20:00.913"), testRow.created);
		}
	}
}