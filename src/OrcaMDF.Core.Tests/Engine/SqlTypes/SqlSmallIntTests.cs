using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlSmallIntTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlSmallInt(CompressionContext.NoCompression);
			byte[] input;

			input = new byte[] { 0x16, 0x33 };
			Assert.AreEqual(13078, Convert.ToInt16(type.GetValue(input)));

			input = new byte[] { 0xf9, 0x0d };
			Assert.AreEqual(3577, Convert.ToInt16(type.GetValue(input)));

			input = new byte[] { 0xa4, 0xd6 };
			Assert.AreEqual(-10588, Convert.ToInt16(type.GetValue(input)));
		}

		[Test]
		public void Length()
		{
			var type = new SqlSmallInt(CompressionContext.NoCompression);

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[1]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[3]));
		}
	}
}