using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlNCharTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlNChar(6, CompressionContext.NoCompression);
			byte[] input;

			input = new byte[] { 0x47, 0x04, 0x2f, 0x04, 0xe6, 0x00 };
			Assert.AreEqual("\u0447\u042f\u00e6", (string)type.GetValue(input));
		}

		[Test]
		public void Length()
		{
			var type = new SqlNChar(6, CompressionContext.NoCompression);

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[7]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[5]));
		}
	}
}