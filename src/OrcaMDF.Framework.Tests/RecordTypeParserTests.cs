using NUnit.Framework;

namespace OrcaMDF.Framework.Tests
{
	[TestFixture]
	public class RecordTypeParserTests
	{
		[Test]
		public void Parse()
		{
			Assert.AreEqual(RecordType.BlobFragment, RecordTypeParser.Parse((int)RecordType.BlobFragment << 1));
			Assert.AreEqual(RecordType.Forwarded, RecordTypeParser.Parse((int)RecordType.Forwarded << 1));
			Assert.AreEqual(RecordType.ForwardingStub, RecordTypeParser.Parse((int)RecordType.ForwardingStub << 1));
			Assert.AreEqual(RecordType.GhostData, RecordTypeParser.Parse((int)RecordType.GhostData << 1));
			Assert.AreEqual(RecordType.GhostIndex, RecordTypeParser.Parse((int)RecordType.GhostIndex << 1));
			Assert.AreEqual(RecordType.GhostVersion, RecordTypeParser.Parse((int)RecordType.GhostVersion << 1));
			Assert.AreEqual(RecordType.Index, RecordTypeParser.Parse((int)RecordType.Index << 1));
			Assert.AreEqual(RecordType.Primary, RecordTypeParser.Parse((int)RecordType.Primary << 1));
		}
	}
}