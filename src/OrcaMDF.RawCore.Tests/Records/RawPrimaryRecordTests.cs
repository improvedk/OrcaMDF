using NUnit.Framework;
using OrcaMDF.RawCore.Records;
using System.Linq;

namespace OrcaMDF.RawCore.Tests.Records
{
	public class RawPrimaryRecordTests : BaseFixture
	{
		[Test]
		public void Parse()
		{
			var db = new RawDatabase(AWPath);
			var page = db.GetPage(1, 1500);
			var record = (RawPrimaryRecord)page.Records.First();

			Assert.AreEqual(true, record.HasNullBitmap);
			Assert.AreEqual(true, record.HasVariableLengthColumns);
			Assert.AreEqual(false, record.HasVersioningInformation);
			Assert.AreEqual(false, record.IsGhostForwardedRecord);
			Assert.AreEqual(0x30, record.RawStatusByteA);
			Assert.AreEqual(0x00, record.RawStatusByteB);
			Assert.AreEqual(false, record.Version);
			Assert.AreEqual(111, record.FixedLengthData.Count);
			Assert.AreEqual(24, record.NullBitmapColumnCount);
			Assert.AreEqual(new[] { 0x80, 0x04, 0x20 }, record.NullBitmapRawBytes.ToArray());
			Assert.AreEqual(3, record.NumberOfVariableLengthOffsetArrayEntries);
			Assert.AreEqual(new[] { 0x0080, 0x009C, 0x00A8 }, record.VariableLengthOffsetArray.ToArray());
			Assert.AreEqual(3, record.VariableLengthOffsetValues.Count());
			Assert.AreEqual(0, record.VariableLengthOffsetValues.ToArray()[0].Count);
			Assert.AreEqual(new[] { 0x31, 0x00, 0x30, 0x00, 0x2d, 0x00, 0x34, 0x00, 0x30, 0x00, 0x33, 0x00, 0x30, 0x00, 0x2d, 0x00, 0x30, 0x00, 0x32, 0x00, 0x39, 0x00, 0x33, 0x00, 0x34, 0x00, 0x32, 0x00 }, record.VariableLengthOffsetValues.ToArray()[1].ToArray());
			Assert.AreEqual(new[] { 0x37, 0x34, 0x33, 0x31, 0x33, 0x38, 0x56, 0x69, 0x31, 0x34, 0x36, 0x33 }, record.VariableLengthOffsetValues.ToArray()[2].ToArray());
		}
	}
}