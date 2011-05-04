using NUnit.Framework;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlVarcharTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlVarchar();
			byte[] input;

			input = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
			Assert.AreEqual("Hello", (string)type.GetValue(input));
		}
	}
}