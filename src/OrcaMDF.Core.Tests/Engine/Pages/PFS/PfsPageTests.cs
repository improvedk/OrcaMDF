using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.Pages.PFS;

namespace OrcaMDF.Core.Tests.Engine.Pages.PFS
{
	[TestFixture]
	public class PfsPageTests
	{
		[Test]
		public void GetPfsPointerForPage()
		{
			Assert.AreEqual(new PagePointer(1, 1), PfsPage.GetPfsPointerForPage(new PagePointer(1, 27)));
			Assert.AreEqual(new PagePointer(1, 1), PfsPage.GetPfsPointerForPage(new PagePointer(1, 0)));
			Assert.AreEqual(new PagePointer(1, 1), PfsPage.GetPfsPointerForPage(new PagePointer(1, 8087)));
			Assert.AreEqual(new PagePointer(1, 8088), PfsPage.GetPfsPointerForPage(new PagePointer(1, 8088)));
			Assert.AreEqual(new PagePointer(1, 8088), PfsPage.GetPfsPointerForPage(new PagePointer(1, 8089)));
		}
	}
}