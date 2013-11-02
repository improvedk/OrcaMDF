using NUnit.Framework;
using OrcaMDF.Framework;

namespace OrcaMDF.RawCore.Tests
{
	public class RawPageHeaderTests : BaseFixture
	{
		[Test]
		public void Parse_Header_2005()
		{
			var db = new RawDatabase(AW2005Path);
			var header = db.GetPage(1, 118).Header;

			Assert.AreEqual(96, header.RawBytes.Count);
			Assert.AreEqual(8040, header.FreeCnt);
			Assert.AreEqual(150, header.FreeData);
			Assert.AreEqual(33280, header.FlagBits);
			Assert.AreEqual("(17:858:20)", header.Lsn);
			Assert.AreEqual(86, header.ObjectID);
			Assert.AreEqual(PageType.Data, header.Type);
			Assert.AreEqual(21, header.Pminlen);
			Assert.AreEqual(256, header.IndexID);
			Assert.AreEqual(0x4, header.TypeFlagBits);
			Assert.AreEqual(1, header.SlotCnt);
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
			Assert.AreEqual(118, header.PageID);
			Assert.AreEqual(1, header.FileID);
			Assert.AreEqual(new [] { 0xDA, 0xC1, 0x11, 0x61 }, header.Checksum.ToArray());
		}

		[Test]
		public void Parse_Header_2008()
		{
			var db = new RawDatabase(AW2008Path);
			var header = db.GetPage(1, 187).Header;

			Assert.AreEqual(96, header.RawBytes.Count);
			Assert.AreEqual(8038, header.FreeCnt);
			Assert.AreEqual(152, header.FreeData);
			Assert.AreEqual(33280, header.FlagBits);
			Assert.AreEqual("(30:848:19)", header.Lsn);
			Assert.AreEqual(31, header.ObjectID);
			Assert.AreEqual(PageType.Data, header.Type);
			Assert.AreEqual(21, header.Pminlen);
			Assert.AreEqual(256, header.IndexID);
			Assert.AreEqual(0x4, header.TypeFlagBits);
			Assert.AreEqual(1, header.SlotCnt);
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
			Assert.AreEqual(187, header.PageID);
			Assert.AreEqual(1, header.FileID);
			Assert.AreEqual(new [] { 0xCA, 0x80, 0x15, 0xFC }, header.Checksum.ToArray());
		}

		[Test]
		public void Parse_Header_2008R2()
		{
			var db = new RawDatabase(AW2008R2Path);
			var header = db.GetPage(1, 184).Header;

			Assert.AreEqual(96, header.RawBytes.Count);
			Assert.AreEqual(8038, header.FreeCnt);
			Assert.AreEqual(152, header.FreeData);
			Assert.AreEqual(544, header.FlagBits);
			Assert.AreEqual("(20:577:120)", header.Lsn);
			Assert.AreEqual(33, header.ObjectID);
			Assert.AreEqual(PageType.Data, header.Type);
			Assert.AreEqual(21, header.Pminlen);
			Assert.AreEqual(256, header.IndexID);
			Assert.AreEqual(0x0, header.TypeFlagBits);
			Assert.AreEqual(1, header.SlotCnt);
			Assert.AreEqual("(0:572)", header.XdesID);
			Assert.AreEqual(0, header.XactReserved);
			Assert.AreEqual(0, header.ReservedCnt);
			Assert.AreEqual(0, header.Level);
			Assert.AreEqual(1, header.HeaderVersion);
			Assert.AreEqual(0, header.GhostRecCnt);
			Assert.AreEqual(0, header.NextPageFileID);
			Assert.AreEqual(0, header.NextPageID);
			Assert.AreEqual(0, header.PreviousPageFileID);
			Assert.AreEqual(0, header.PreviousPageID);
			Assert.AreEqual(184, header.PageID);
			Assert.AreEqual(1, header.FileID);
			Assert.AreEqual(new [] { 0xC8, 0x00, 0xBD, 0xBD }, header.Checksum.ToArray());
		}

		[Test]
		public void Parse_Header_2012()
		{
			var db = new RawDatabase(AW2012Path);
			var header = db.GetPage(1, 187).Header;

			Assert.AreEqual(96, header.RawBytes.Count);
			Assert.AreEqual(8042, header.FreeCnt);
			Assert.AreEqual(152, header.FreeData);
			Assert.AreEqual(33280, header.FlagBits);
			Assert.AreEqual("(48:49:2)", header.Lsn);
			Assert.AreEqual(31, header.ObjectID);
			Assert.AreEqual(PageType.Data, header.Type);
			Assert.AreEqual(21, header.Pminlen);
			Assert.AreEqual(256, header.IndexID);
			Assert.AreEqual(0x4, header.TypeFlagBits);
			Assert.AreEqual(1, header.SlotCnt);
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
			Assert.AreEqual(187, header.PageID);
			Assert.AreEqual(1, header.FileID);
			Assert.AreEqual(new [] { 0xC2, 0x00, 0xB4, 0x7D }, header.Checksum.ToArray());
		}
	}
}