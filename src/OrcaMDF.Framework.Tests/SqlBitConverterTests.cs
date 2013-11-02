using NUnit.Framework;

namespace OrcaMDF.Framework.Tests
{
	[TestFixture]
	public class SqlBitConverterTests
	{
		[Test]
		public void ToInt16FromBigEndian()
		{
			Assert.AreEqual(0, SqlBitConverter.ToInt16FromBigEndian(new byte[] { 0x0 }, 0, Offset.Zero));
			Assert.AreEqual(239, SqlBitConverter.ToInt16FromBigEndian(new byte[] { 0xEF }, 0, Offset.Zero));
			Assert.AreEqual(111, SqlBitConverter.ToInt16FromBigEndian(new byte[] { 0xEF }, 0, Offset.MinValue));
			Assert.AreEqual(-112, SqlBitConverter.ToInt16FromBigEndian(new byte[] { 0x10 }, 0, Offset.MinValue));
			Assert.AreEqual(20239, SqlBitConverter.ToInt16FromBigEndian(new byte[] { 0x4F, 0x0F }, 0, Offset.Zero));
		}

		[Test]
		public void ToInt32FromBigEndian()
		{
			Assert.AreEqual(130, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x82 }, 0, Offset.Zero));
			Assert.AreEqual(32767, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x7F, 0xFF }, 0, Offset.Zero));
			Assert.AreEqual(32768, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x80, 0x00 }, 0, Offset.Zero));
			Assert.AreEqual(8388607, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x7F, 0xFF, 0xFF }, 0, Offset.Zero));
			Assert.AreEqual(2147483647, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x7F, 0xFF, 0xFF, 0xFF }, 0, Offset.Zero));
			Assert.AreEqual(-125, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x03 }, 0, Offset.MinValue));
			Assert.AreEqual(-32768, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x00, 0x00 }, 0, Offset.MinValue));
			Assert.AreEqual(-8388608, SqlBitConverter.ToInt32FromBigEndian(new byte[] { 0x00, 0x00, 0x00 }, 0, Offset.MinValue));
		}

		[Test]
		public void ToInt64FromBigEndian()
		{
			Assert.AreEqual(9223372036854775807, SqlBitConverter.ToInt64FromBigEndian(TestHelper.GetBytesFromByteString("FFFFFFFFFFFFFFFF"), 0, Offset.MinValue));
			Assert.AreEqual(-9223372036854775808, SqlBitConverter.ToInt64FromBigEndian(TestHelper.GetBytesFromByteString("0000000000000000"), 0, Offset.MinValue));
			Assert.AreEqual(-8877493061872343484, SqlBitConverter.ToInt64FromBigEndian(TestHelper.GetBytesFromByteString("04CCCF185F1AA644"), 0, Offset.MinValue));
			Assert.AreEqual(-4611686018427387904, SqlBitConverter.ToInt64FromBigEndian(TestHelper.GetBytesFromByteString("4000000000000000"), 0, Offset.MinValue));
			Assert.AreEqual(461168601842738790, SqlBitConverter.ToInt64FromBigEndian(TestHelper.GetBytesFromByteString("8666666666666666"), 0, Offset.MinValue));
		}
	}
}