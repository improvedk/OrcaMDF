using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Tests.Engine.Pages
{
	[TestFixture]
	public class GamPageTests
	{
		[Test]
		public void GetGamPointerForPage()
		{
			Assert.AreEqual(new PagePointer(1, 2), GamPage.GetGamPointerForPage(new PagePointer(1, 27)));
			Assert.AreEqual(new PagePointer(1, 2), GamPage.GetGamPointerForPage(new PagePointer(1, 0)));
			Assert.AreEqual(new PagePointer(1, 2), GamPage.GetGamPointerForPage(new PagePointer(1, 511231)));
			Assert.AreEqual(new PagePointer(1, 511232), GamPage.GetGamPointerForPage(new PagePointer(1, 511232)));
			Assert.AreEqual(new PagePointer(1, 511232), GamPage.GetGamPointerForPage(new PagePointer(1, 511233)));
		}
	}
}