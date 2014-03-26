using NUnit.Framework;
using OrcaMDF.Framework;
using OrcaMDF.RawCore.Records;
using OrcaMDF.RawCore.Types;

namespace OrcaMDF.RawCore.Tests.Records
{
	public class RawIndexRecordTests : BaseFixture
	{
		[Test]
		public void BranchLevelNonClusteredWithNullBitmapAndVariableLength2012_FirstRecord()
		{
			var rawBytes = TestHelper.GetBytesFromByteString(@"
				36ca0000 00760200 00010002 00000100 3e006100
				30004000 61006400 76006500 6e007400 75007200
				65002d00 77006f00 72006b00 73002e00 63006f00
				6d00
			");

			var record = new RawIndexRecord(new ArrayDelimiter<byte>(rawBytes), 11, 1);

			// First record has implicit null values
			Assert.AreEqual(1, record.ChildFileID);
			Assert.AreEqual(630, record.ChildPageID);
		}

		[Test]
		public void BranchLevelNonClusteredWithNullBitmapAndVariableLength2012_SecondRecord()
		{
			var rawBytes = TestHelper.GetBytesFromByteString(@"
				36d90000 00780200 00010002 00000100 46006300
				68007200 69007300 36004000 61006400 76006500
				6e007400 75007200 65002d00 77006f00 72006b00
				73002e00 63006f00 6d00
			");

			var record = new RawIndexRecord(new ArrayDelimiter<byte>(rawBytes), 11, 1);

			Assert.AreEqual(1, record.ChildFileID);
			Assert.AreEqual(632, record.ChildPageID);

			dynamic row = RawColumnParser.Parse(record, new IRawType[] {
				RawType.NVarchar("EmailAddress"),
				RawType.Int("CustomerID")
			});

			Assert.AreEqual("chris6@adventure-works.com", row.EmailAddress);
			Assert.AreEqual(217, row.CustomerID);
		}

		[Test]
		public void LeafLevelNonclusteredWithNullBitmapAndVariableLength2012_A()
		{
			var rawBytes = TestHelper.GetBytesFromByteString(@"
				36a50100 00020000 01004400 66006f00 72007200
				65007300 74003000 40006100 64007600 65006e00
				74007500 72006500 2d007700 6f007200 6b007300
				2e006300 6f006d00
			");

			var record = new RawIndexRecord(new ArrayDelimiter<byte>(rawBytes), 5, 0);

			Assert.AreEqual(4, record.FixedLengthData.Count);
			Assert.AreEqual(true, record.HasNullBitmap);
			Assert.AreEqual(true, record.HasVariableLengthColumns);
			Assert.AreEqual(2, record.NullBitmapColumnCount);
			Assert.IsNull(record.ChildPageID);
			Assert.IsNull(record.ChildFileID);

			dynamic row = RawColumnParser.Parse(record, new IRawType[] {
				RawType.NVarchar("EmailAddress"),
				RawType.Int("CustomerID")
			});

			Assert.AreEqual("forrest0@adventure-works.com", row.EmailAddress);
			Assert.AreEqual(421, row.CustomerID);
		}

		[Test]
		public void LeafLevelNonclusteredWithNullBitmapAndVariableLength2012_B()
		{
			var rawBytes = TestHelper.GetBytesFromByteString(@"
				36947500 00020000 01004200 6a006500 73007300
				69006500 30004000 61006400 76006500 6e007400
				75007200 65002d00 77006f00 72006b00 73002e00
				63006f00 6d00
			");

			var record = new RawIndexRecord(new ArrayDelimiter<byte>(rawBytes), 5, 0);

			Assert.AreEqual(4, record.FixedLengthData.Count);
			Assert.AreEqual(true, record.HasNullBitmap);
			Assert.AreEqual(true, record.HasVariableLengthColumns);
			Assert.AreEqual(2, record.NullBitmapColumnCount);
			Assert.IsNull(record.ChildPageID);
			Assert.IsNull(record.ChildFileID);

			dynamic row = RawColumnParser.Parse(record, new IRawType[] {
				RawType.NVarchar("EmailAddress"),
				RawType.Int("CustomerID")
			});

			Assert.AreEqual("jessie0@adventure-works.com", row.EmailAddress);
			Assert.AreEqual(30100, row.CustomerID);
		}
	}
}