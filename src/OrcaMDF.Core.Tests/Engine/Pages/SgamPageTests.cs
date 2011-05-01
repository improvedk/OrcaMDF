using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Tests.Engine.Pages
{
	[TestFixture]
	public class SgamPageTests
	{
		[Test]
		public void GetSgamPointerForPage()
		{
			Assert.AreEqual(new PagePointer(1, 3), SgamPage.GetSgamPointerForPage(new PagePointer(1, 27)));
			Assert.AreEqual(new PagePointer(1, 3), SgamPage.GetSgamPointerForPage(new PagePointer(1, 0)));
			Assert.AreEqual(new PagePointer(1, 511233), SgamPage.GetSgamPointerForPage(new PagePointer(1, 511232)));
			Assert.AreEqual(new PagePointer(1, 511233), SgamPage.GetSgamPointerForPage(new PagePointer(1, 511233)));
			Assert.AreEqual(new PagePointer(1, 511233), SgamPage.GetSgamPointerForPage(new PagePointer(1, 511234)));
		}
	}
}