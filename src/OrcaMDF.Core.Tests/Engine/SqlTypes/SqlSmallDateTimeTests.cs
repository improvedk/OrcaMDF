using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlSmallDateTimeTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlSmallDateTime(CompressionContext.NoCompression);

			var input = new byte[] { 0xab, 0x02, 0x5d, 0x26 };
			Assert.AreEqual(new DateTime(1926, 11, 22, 11, 23, 0), Convert.ToDateTime(type.GetValue(input)));

			input = new byte[] { 0x49, 0x03, 0x99, 0x09 };
			Assert.AreEqual(new DateTime(1906, 9, 24, 14, 1, 0), Convert.ToDateTime(type.GetValue(input)));
		}

		[Test]
		public void Length()
		{
			var type = new SqlSmallDateTime(CompressionContext.NoCompression);

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[3]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[5]));
		}
	}
}