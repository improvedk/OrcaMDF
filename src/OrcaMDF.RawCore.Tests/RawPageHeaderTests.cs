using NUnit.Framework;

namespace OrcaMDF.RawCore.Tests
{
	public class RawPageHeaderTests : BaseFixture
	{
		[Test]
		public void Parse_IAM()
		{
			var db = new RawDatabase(AWPath);
			var header = db.GetPage(1, 100).Header;

			Assert.AreEqual(96, header.RawBytes.Count);
			Assert.AreEqual(6, header.FreeCnt);
			Assert.AreEqual(8182, header.FreeData);
			Assert.AreEqual(0x200, header.FlagBits);
			Assert.AreEqual("(41:15703:39)", header.Lsn);
			Assert.AreEqual(74, header.ObjectID);
			Assert.AreEqual(RawPageType.IAM, header.Type);
			Assert.AreEqual(90, header.Pminlen);
			Assert.AreEqual(2, header.IndexID);
			Assert.AreEqual(0x0, header.TypeFlagBits);
			Assert.AreEqual(2, header.SlotCnt);
			Assert.AreEqual("(0:0)", header.XdesID);
			Assert.AreEqual(0, header.XactReserved);
			Assert.AreEqual(0, header.ReservedCnt);
			Assert.AreEqual(0, header.Level);
			Assert.AreEqual(1, header.HeaderVersion);
			Assert.AreEqual(0, header.GhostRecCnt);
			Assert.AreEqual(0, header.NextPageFileID);
			Assert.AreEqual(0, header.NextPageID);
			Assert.AreEqual(0, header.PreviousPageFileID);
			Assert.AreEqual(0, header.PreviousPageID);
			Assert.AreEqual(100, header.PageID);
			Assert.AreEqual(1, header.FileID);
			Assert.AreEqual(new [] { 0x90, 0x28, 0xA2, 0x22 }, header.Checksum.ToArray());
		}
	}
}