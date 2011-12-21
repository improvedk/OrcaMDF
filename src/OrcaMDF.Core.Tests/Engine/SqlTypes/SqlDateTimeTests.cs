using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlDateTimeTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlDateTime(CompressionContext.NoCompression);
			byte[] input;

			input = new byte[] { 0x5e, 0x3b, 0x5d, 0x00, 0x25, 0x91, 0x00, 0x00 };
			Assert.AreEqual(new DateTime(2001, 09, 25, 05, 39, 26, 820), (DateTime)type.GetValue(input));

			input = new byte[] { 0xb6, 0x87, 0xf0, 0x00, 0xd1, 0x8b, 0x00, 0x00 };
			Assert.AreEqual(new DateTime(1997, 12, 31, 14, 35, 44, 607), (DateTime)type.GetValue(input));

			input = new byte[] { 0x2d, 0xfd, 0x1c, 0x01, 0x4a, 0x75, 0x00, 0x00 };
			Assert.AreEqual(new DateTime(1982, 03, 18, 17, 17, 36, 790), (DateTime)type.GetValue(input));

			input = new byte[] { 0xff, 0x81, 0x8b, 0x01, 0x7f, 0x24, 0x2d, 0x00 };
			Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59, 997), (DateTime)type.GetValue(input));
		}

		[Test]
		public void Length()
		{
			var type = new SqlDateTime(CompressionContext.NoCompression);

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[9]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[7]));
		}
	}
}