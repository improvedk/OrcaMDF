using NUnit.Framework;
using OrcaMDF.Core.Engine.Records.Compression;

namespace OrcaMDF.Core.Tests.Engine.Records
{
	[TestFixture]
	public class CompressedRecordParserTests
	{
		[Test]
		public void SingleIntColumnWithValue1()
		{
			byte[] input = TestHelper.GetBytesFromByteString("01011281 00000000 10");

			var parser = new CompressedRecordParser(input);

			Assert.AreEqual(parser.NumberOfColumns, 1);
			Assert.IsFalse(parser.HasVersioningInformation);
			Assert.AreEqual(CompressedRecordType.Primary, parser.RecordType);
			Assert.AreEqual(CompressedRecordFormat.CD, parser.RecordFormat);
			
			Assert.AreEqual(1, (sbyte)parser.GetPhysicalColumnValue(0)[0]);
		}

		[Test]
		public void SingleIntColumnWithValueNegativeOne()
		{
			byte[] input = TestHelper.GetBytesFromByteString("0101127f 00000000 00");

			var parser = new CompressedRecordParser(input);

			Assert.AreEqual(parser.NumberOfColumns, 1);
			Assert.AreEqual(-1, (sbyte)parser.GetPhysicalColumnValue(0)[0]);
		}

		[Test]
		public void SingleIntColumnWithValueZero()
		{
			byte[] input = TestHelper.GetBytesFromByteString("01011100 00000000 00");

			var parser = new CompressedRecordParser(input);

			Assert.AreEqual(parser.NumberOfColumns, 1);
			Assert.AreEqual(-1, (sbyte)parser.GetPhysicalColumnValue(0)[0]);
		}

		[Test]
		public void SingleIntColumnWithValueNull()
		{
			byte[] input = TestHelper.GetBytesFromByteString("01011084 00000000 a8");

			var parser = new CompressedRecordParser(input);

			Assert.AreEqual(parser.NumberOfColumns, 1);
			Assert.AreEqual(null, parser.GetPhysicalColumnValue(0));
		}
	}
}