using NUnit.Framework;
using OrcaMDF.Core.Engine.Records;

namespace OrcaMDF.Core.Tests.Engine.Records
{
	[TestFixture]
	public class PrimaryRecordTests
	{
		[Test]
		public void TestNoVarLength()
		{
			var a = new byte[] { 0x10, 0x00, 0x06, 0x00, 0x16, 0x33,0x01, 0x00, 0x00 };
			var record = new PrimaryRecord(a, null);

			Assert.IsFalse(record.IsGhostForwardedRecord);
			Assert.IsTrue(record.HasNullBitmap);
			Assert.AreEqual(RecordType.Primary, record.Type);
			Assert.AreEqual(2, record.FixedLengthData.Length);
			Assert.IsFalse(record.HasVariableLengthColumns);
			Assert.IsFalse(record.HasVersioningInformation);
		}

		[Test]
		public void TestFixedAndVarLength()
		{
			var a = new byte[] { 0x30, 0x00, 0x06, 0x00, 0x02, 0x00, 0x02, 0x00, 0x00, 0x01, 0x00, 0x11, 0x00, 0x6d, 0x61, 0x72, 0x6b };
			var record = new PrimaryRecord(a, null);

			Assert.IsFalse(record.IsGhostForwardedRecord);
			Assert.IsTrue(record.HasNullBitmap);
			Assert.AreEqual(RecordType.Primary, record.Type);
			Assert.AreEqual(2, record.FixedLengthData.Length);
			Assert.IsTrue(record.HasVariableLengthColumns);
			Assert.IsFalse(record.HasVersioningInformation);
			Assert.AreEqual(1, record.NumberOfVariableLengthColumns);
		}

		[Test]
		public void TestGhostForwardedRecord()
		{
			var a = new byte[] { 0x30, 0x01, 0x06, 0x00, 0x02, 0x00, 0x02, 0x00, 0x00, 0x01, 0x00, 0x11, 0x00, 0x6d, 0x61, 0x72, 0x6b };
			var record = new PrimaryRecord(a, null);

			Assert.IsTrue(record.HasNullBitmap);
			Assert.IsTrue(record.IsGhostForwardedRecord);
			Assert.AreEqual(RecordType.Primary, record.Type);
			Assert.AreEqual(2, record.FixedLengthData.Length);
			Assert.IsTrue(record.HasVariableLengthColumns);
			Assert.IsFalse(record.HasVersioningInformation);
		}
	}
}