using NUnit.Framework;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlVarBinaryTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlVarBinary();
			byte[] input;

			input = new byte[] { 0x25, 0xF8, 0x32 };
			Assert.AreEqual(new byte[] { 0x25, 0xF8, 0x32 }, type.GetValue(input));
		}
	}
}