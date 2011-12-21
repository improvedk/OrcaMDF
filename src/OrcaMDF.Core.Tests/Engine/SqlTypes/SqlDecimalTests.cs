using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlDecimalTests
	{
		[Test]
		public void GetValue()
		{
			SqlDecimal type;
			byte[] input;

			type = new SqlDecimal(5, 0, CompressionContext.NoCompression);
			input = new byte[] { 0x01, 0x39, 0x30, 0x00, 0x00 };
			Assert.AreEqual(12345m, type.GetValue(input));

			type = new SqlDecimal(5, 3, CompressionContext.NoCompression);
			input = new byte[] { 0x01, 0x39, 0x30, 0x00, 0x00 };
			Assert.AreEqual(12.345m, type.GetValue(input));

			type = new SqlDecimal(5, 3, CompressionContext.NoCompression);
			input = new byte[] { 0x00, 0x39, 0x30, 0x00, 0x00 };
			Assert.AreEqual(-12.345m, type.GetValue(input));

			type = new SqlDecimal(9, 1, CompressionContext.NoCompression);
			input = new byte[] { 0x01, 0x4e, 0xe4, 0x01, 0x00 };
			Assert.AreEqual(12398.2m, type.GetValue(input));

			type = new SqlDecimal(17, 5, CompressionContext.NoCompression);
			input = new byte[] { 0x01, 0xb9, 0xe3, 0x5d, 0xb6, 0x40, 0x70, 0x00, 0x00 };
			Assert.AreEqual(1234232398.24313m, type.GetValue(input));
		}

		[Test]
		public void Length()
		{
			var type = new SqlDecimal(38, 2, CompressionContext.NoCompression);
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[7]));
			type.GetValue(new byte[17]);

			type = new SqlDecimal(15, 13, CompressionContext.NoCompression);
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[7]));
			type.GetValue(new byte[9]);
		}
	}
}