using NUnit.Framework;
using OrcaMDF.Core.Engine.Records.Compression;

namespace OrcaMDF.Core.Tests.Engine.Records
{
	[TestFixture]
	public class CompressedRecordParserTests
	{
		 [Test]
		 public void SingleIntColumn()
		 {
		 	byte[] input = TestHelper.GetBytesFromByteString("01011288 00000000 10");

		 	var parser = new CompressedRecordParser(input);

		 	Assert.AreEqual(parser.NumberOfColumns, 1);
		 	Assert.IsFalse(parser.ContainsLongDataRegion);
		 	Assert.IsFalse(parser.HasVersioningInformation);
		 	Assert.AreEqual(CompressedRecordType.Primary, parser.RecordType);
		 	Assert.AreEqual(CompressedRecordFormat.CD, parser.RecordFormat);
		 }
	}
}