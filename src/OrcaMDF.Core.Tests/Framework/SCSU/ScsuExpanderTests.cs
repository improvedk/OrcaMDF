using NUnit.Framework;
using OrcaMDF.Core.Framework.SCSU;

namespace OrcaMDF.Core.Tests.Framework.SCSU
{
	[TestFixture]
	public class ScsuExpanderTests
	{
		[Test]
		public void Test()
		{
			var x = new ScsuExpander();

			Assert.AreEqual("a", x.Expand(TestHelper.GetBytesFromByteString("61")));
			Assert.AreEqual("", x.Expand(TestHelper.GetBytesFromByteString("")));
			Assert.AreEqual("123", x.Expand(TestHelper.GetBytesFromByteString("313233")));
			Assert.AreEqual("ѨѨѨѨѨѨѨѨѨѨ", x.Expand(TestHelper.GetBytesFromByteString("12E8E8E8 E8E8E8E8 E8E8E8")));
			Assert.AreEqual("An example sentence would show what this word means", x.Expand(TestHelper.GetBytesFromByteString("416E2065 78616D70 6C652073 656E7465 6E636520 776F756C 64207368 6F772077 68617420 74686973 20776F72 64206D65 616E73")));
			Assert.AreEqual("～であれ～であれ", x.Expand(TestHelper.GetBytesFromByteString("08DE15A7 82CC08DE A782CC")));
			Assert.AreEqual("でででAAAAAAででAでX", x.Expand(TestHelper.GetBytesFromByteString("15A7A7A7 41414141 4141A7A7 41A758")));
			Assert.AreEqual("".PadLeft(20, ' '), x.Expand(TestHelper.GetBytesFromByteString("20202020 20202020 20202020 20202020 20202020 10")));
			Assert.AreEqual("1234567890", x.Expand(TestHelper.GetBytesFromByteString("31323334 35363738 393010")));
		}
	}
}