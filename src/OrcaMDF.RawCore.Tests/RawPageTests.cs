using System.Linq;
using NUnit.Framework;

namespace OrcaMDF.RawCore.Tests
{
	public class RawPageTests : BaseFixture
	{
		[Test]
		public void Parse_Data()
		{
			var db = new RawDatabase(AWPath);
			var page = db.GetPage(1, 1500);

			Assert.AreEqual(1, page.FileID);
			Assert.AreEqual(1500 * 8192, page.DataFileIndex);
			Assert.AreEqual(1500, page.PageID);
			Assert.AreEqual(8192, page.RawBytes.Count);
			Assert.AreEqual(47, page.SlotArray.Count());
			Assert.AreEqual(47, page.Records.Count());
		}
	}
}