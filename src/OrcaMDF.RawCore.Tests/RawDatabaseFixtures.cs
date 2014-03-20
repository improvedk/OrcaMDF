using NUnit.Framework;
using System.Linq;

namespace OrcaMDF.RawCore.Tests
{
	public class RawDatabaseFixtures : BaseFixture
	{
		[TestCase(AW2005Path, 640, TestName = "2005")]
		[TestCase(AW2008Path, 1064, TestName = "2008")]
		[TestCase(AW2008R2Path, 664, TestName = "2008R2")]
		[TestCase(AW2012Path, 1064, TestName = "2012")]
		public void Constructor(string dbPath, int expectedPageCount)
		{
			var db = new RawDataFile(dbPath);

			Assert.AreEqual(expectedPageCount, db.Pages.Count());
		}
	}
}