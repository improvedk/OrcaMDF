using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlBinaryTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlBinary(3);
			byte[] input;

			input = new byte[] { 0x25, 0xF8, 0x32, 0x11 };
			Assert.AreEqual(new byte[] { 0x25, 0xF8, 0x32 }, type.GetValue(input));
		}

		[Test]
		public void GetValue_ThrowsIfNotEnoughData()
		{
			var type = new SqlBinary(5);
			byte[] input;

			input = new byte[] { 0x25, 0xF8, 0x32, 0x11 };
			Assert.Throws<ArgumentException>(() => type.GetValue(input));
		}
	}
}