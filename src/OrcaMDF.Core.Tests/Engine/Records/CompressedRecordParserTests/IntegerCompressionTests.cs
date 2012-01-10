using NUnit.Framework;
using OrcaMDF.Core.Engine.Records.Compression;

namespace OrcaMDF.Core.Tests.Engine.Records.CompressedRecordParserTests
{
	[TestFixture]
	public class IntegerCompressionTests
	{
		[Test]
		public void TinyInt()
		{
			CompressedRecord parser;
			
			// 1
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011201 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// 127
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("0101127f 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// 128
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011280 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// 255
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010112ff 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// NULL
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011084 00000000 a8"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(null, parser.GetPhysicalColumnBytes(0));
		}

		[Test]
		public void SmallInt()
		{
			CompressedRecord parser;
			
			// NULL
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011083 00000000 a8"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(null, parser.GetPhysicalColumnBytes(0));
			
			// 1
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011281 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// -1
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("0101127f 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// 127
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010112ff 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// 128
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011380 80000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(2, parser.GetPhysicalColumnBytes(0).Length);
			
			// 255
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011380 ff000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(2, parser.GetPhysicalColumnBytes(0).Length);
			
			// -32768
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011300 00000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(2, parser.GetPhysicalColumnBytes(0).Length);
		}

		[Test]
		public void Int()
		{
			CompressedRecord parser;
			
			// 127
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010112ff 00000000 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);
			
			// 128
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011380 80000000 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(2, parser.GetPhysicalColumnBytes(0).Length);
			
			// 32767
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010113ff ff000000 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(2, parser.GetPhysicalColumnBytes(0).Length);
			
			// 32768
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011480 80000000 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(3, parser.GetPhysicalColumnBytes(0).Length);

			// 8388607
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010114ff ffff0000 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(3, parser.GetPhysicalColumnBytes(0).Length);

			// 2147483647
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010115ff ffffff00 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(4, parser.GetPhysicalColumnBytes(0).Length);

			// -128
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011200 00000000 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(1, parser.GetPhysicalColumnBytes(0).Length);

			// -129
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("0101137f 7f000000 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(2, parser.GetPhysicalColumnBytes(0).Length);

			// -8388608
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011400 00000000 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(3, parser.GetPhysicalColumnBytes(0).Length);

			// -8388609
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("0101157f 7fffff00 10"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(4, parser.GetPhysicalColumnBytes(0).Length);
		}

		[Test]
		public void BigInt()
		{
			CompressedRecord parser;

			// 9223372036854775807
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010119ff ffffffff ffffff"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(8, parser.GetPhysicalColumnBytes(0).Length);

			// 36028797018963967
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010118ff ffffffff ffff"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(7, parser.GetPhysicalColumnBytes(0).Length);

			// 140737488355327
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010117ff ffffffff ff"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(6, parser.GetPhysicalColumnBytes(0).Length);

			// 549755813887
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010116ff ffffffff 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(5, parser.GetPhysicalColumnBytes(0).Length);

			// 2147483647
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("010115ff ffffff00 00"));
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(4, parser.GetPhysicalColumnBytes(0).Length);
		}
	}
}