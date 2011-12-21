using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlBinaryTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlBinary(3, CompressionContext.NoCompression);
			byte[] input;

			input = new byte[] { 0x25, 0xF8, 0x32 };
			Assert.AreEqual(new byte[] { 0x25, 0xF8, 0x32 }, type.GetValue(input));
		}

		[Test]
		public void Length()
		{
			var type = new SqlBinary(5, CompressionContext.NoCompression);
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[6]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[4]));
		}
	}
}