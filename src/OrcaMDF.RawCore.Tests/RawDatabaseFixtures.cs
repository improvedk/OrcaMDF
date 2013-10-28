using NUnit.Framework;
using System.Linq;

namespace OrcaMDF.RawCore.Tests
{
	public class RawDatabaseFixtures : BaseFixture
	{
		[Test]
		public void Constructor()
		{
			var db = new RawDatabase(AWPath);

			Assert.AreEqual(25080, db.Pages.Count());
		}
	}
}