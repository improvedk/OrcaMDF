using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine.Records.Compression;

namespace OrcaMDF.Core.Tests.Engine.Records.CompressedRecordParserTests
{
	[TestFixture]
	public class DateCompressionTests
	{
		 [Test]
		 public void Datetime()
		 {
			CompressedRecord parser;
			
			// 2012-01-29 23:57:42.997
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011880 9FE7018A E173"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(7, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// 2012-01-29 23:57:42.447
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011880 9FE7018A E0CE"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(7, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// 2099-12-31 23:59:59.997
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011881 1D58018B 81FF"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(7, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// 1753-01-01 00:00:00.000
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("0101187F 2E460000 0000"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(7, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// NULL
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011084 00000000 A8"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(null, parser.GetPhysicalColumnBytes(0));

			// 1900-01-01 00:00:00.000
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011100 01010100 01"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(0, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// 1900-01-02 00:00:00.000
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011681 00000000 01"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(5, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// 1900-01-02 18:22:11.123
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011681 012EB969 01"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(5, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// 1900-01-01 22:17:21.447
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("01011581 6F50F200 00"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(4, parser.GetPhysicalColumnBytes(0).GetBytes().Count());

			// 1899-01-02 18:22:11.123
			parser = new CompressedRecord(TestHelper.GetBytesFromByteString("0101177E 94012EB9 69"), null);
			Assert.AreEqual(1, parser.NumberOfColumns);
			Assert.AreEqual(6, parser.GetPhysicalColumnBytes(0).GetBytes().Count());
		 }
	}
}