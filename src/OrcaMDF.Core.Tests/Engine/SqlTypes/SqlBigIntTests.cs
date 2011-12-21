using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlBigIntTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlBigInt(CompressionContext.NoCompression);
			byte[] input;

			input = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F };
			Assert.AreEqual(9223372036854775807, Convert.ToInt64(type.GetValue(input)));

			input = new byte[] { 0x82, 0x5A, 0x03, 0x1B, 0xD5, 0x3E, 0xCD, 0x71 };
			Assert.AreEqual(8200279581513702018, Convert.ToInt64(type.GetValue(input)));

			input = new byte[] { 0x7F, 0xA5, 0xFC, 0xE4, 0x2A, 0xC1, 0x32, 0x8E };
			Assert.AreEqual(-8200279581513702017, Convert.ToInt64(type.GetValue(input)));
		}

		[Test]
		public void Length()
		{
			var type = new SqlBigInt(CompressionContext.NoCompression);

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[9]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[7]));
		}
	}
}