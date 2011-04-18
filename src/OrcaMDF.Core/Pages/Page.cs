using System;
using System.Linq;
using System.Text;
using OrcaMDF.Core.Pages.PFS;

namespace OrcaMDF.Core.Pages
{
	public class Page
	{
		protected MdfFile File;

		// Raw content of the page (8192 bytes)
		public byte[] RawBytes { get; private set; }

		// Header fields
		public short FreeCnt { get; private set; }
		public short FreeData { get; private set; }
		public short FlagBits { get; private set; }
		public string Lsn { get; private set; }
		public int ObjectID { get; private set; }
		public int PageID { get; private set; }
		public short FileID { get; private set; }
		public PageType Type { get; private set; }
		public short Pminlen { get; private set; }
		public short IndexID { get; private set; }
		public byte TypeFlagBits { get; private set; }
		public short SlotCnt { get; private set; }
		public int NextPageID { get; private set; }
		public short NextPageFileID { get; private set; }
		public int PreviousPageID { get; private set; }
		public short PreviousFileID { get; private set; }
		public string XdesID { get; private set; }
		public short XactReserved { get; private set; }
		public short ReservedCnt { get; private set; }
		public byte Level { get; private set; }
		public byte HeaderVersion { get; private set; }
		public short GhostRecCnt { get; private set; }

		public Page(byte[] bytes, MdfFile file)
		{
			if (bytes.Length != 8192)
				throw new ArgumentException("bytes");

			this.File = file;
			RawBytes = bytes;

			parseHeader();
		}

		public GamPage GetGamPage()
		{
			return File.GetGamPage(GamPage.GetGamIndexByPageIndex(PageID));
		}

		public SgamPage GetSgamPage()
		{
			return File.GetSgamPage(SgamPage.GetSgamIndexByPageIndex(PageID));
		}

		public PfsPage GetPfsPage()
		{
			return File.GetPfsPage(PfsPage.GetPfsIndexByPageIndex(PageID));
		}

		private void parseHeader()
		{
			/*
				Bytes	Content
				-----	-------
				00:		HeaderVersion (byte)
				01:		Type (byte)
				02:		TypeFlagBits (byte)
				03:		Level (byte)
				04-05:	FlagBits (int16)
				06-07:	IndexID (int16)
				08-11:	PreviousPageID (int32)
				12-13:	PreviousFileID (int16)
				14-15:	Pminlen (int16)
				16-19:	NextPageID (int32)
				20-21:	NextPageFileID (int16)
				22-23:	SlotCnt (int16)
				24-27:	ObjectID (int32)
				28-29:	FreeCnt (int16)
				30-31:	FreeData (int16)
				32-35:	PageID (int32)
				36-37:	FileID (int16)
				38-39:	ReservedCnt (int16)
				40-43:	Lsn1 (int32)
				44-47:	Lsn2 (int32)
				48-49:	Lsn3 (int16)
				50-51:	XactReserved (int16)
				52-55:	XdesIDPart2 (int32)
				56-57:	XdesIDPart1 (int16)
				58-59:	GhostRecCnt (int16)
				60-95:	?
			*/

			HeaderVersion = RawHeader[0];
			Type = (PageType)RawHeader[1];
			TypeFlagBits = RawHeader[2];
			Level = RawHeader[3];
			FlagBits = BitConverter.ToInt16(RawHeader, 4);
			IndexID = BitConverter.ToInt16(RawHeader, 6);
			PreviousPageID = BitConverter.ToInt32(RawHeader, 8);
			PreviousFileID = BitConverter.ToInt16(RawHeader, 12);
			Pminlen = BitConverter.ToInt16(RawHeader, 14);
			NextPageID = BitConverter.ToInt32(RawHeader, 16);
			NextPageFileID = BitConverter.ToInt16(RawHeader, 20);
			SlotCnt = BitConverter.ToInt16(RawHeader, 22);
			ObjectID = BitConverter.ToInt32(RawHeader, 24);
			FreeCnt = BitConverter.ToInt16(RawHeader, 28);
			FreeData = BitConverter.ToInt16(RawHeader, 30);
			PageID = BitConverter.ToInt32(RawHeader, 32);
			FileID = BitConverter.ToInt16(RawHeader, 36);
			ReservedCnt = BitConverter.ToInt16(RawHeader, 38);
			Lsn = "(" + BitConverter.ToInt32(RawHeader, 40) + ":" + BitConverter.ToInt32(RawHeader, 44) + ":" + BitConverter.ToInt16(RawHeader, 48) + ")";
			XactReserved = BitConverter.ToInt16(RawHeader, 50);
			XdesID = "(" + BitConverter.ToInt16(RawHeader, 56) + ":" + BitConverter.ToInt32(RawHeader, 52) + ")";
			GhostRecCnt = BitConverter.ToInt16(RawHeader, 58);
		}

		public byte[] RawHeader
		{
			get { return RawBytes.Take(96).ToArray(); }
		}

		public byte[] RawBody
		{
			get { return RawBytes.Skip(96).ToArray(); }
		}

		public override string  ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("m_freeCnt:\t" + FreeCnt);
			sb.AppendLine("m_freeData:\t" + FreeData);
			sb.AppendLine("m_flagBits:\t0x" + FlagBits.ToString("x"));
			sb.AppendLine("m_lsn:\t\t" + Lsn);
			sb.AppendLine("m_objId:\t" + ObjectID);
			sb.AppendLine("m_pageId:\t(" + FileID + ":" + PageID + ")");
			sb.AppendLine("m_type:\t\t" + Type);
			sb.AppendLine("m_typeFlagBits:\t" + "0x" + TypeFlagBits.ToString("x"));
			sb.AppendLine("pminlen:\t" + Pminlen);
			sb.AppendLine("m_indexId:\t" + IndexID);
			sb.AppendLine("m_slotCnt:\t" + SlotCnt);
			sb.AppendLine("m_nextPage:\t(" + NextPageFileID + ":" + NextPageID + ")");
			sb.AppendLine("m_prevPage:\t(" + PreviousFileID + ":" + PreviousPageID + ")");
			sb.AppendLine("m_xactReserved:\t" + XactReserved);
			sb.AppendLine("m_xdesId:\t" + XdesID);
			sb.AppendLine("m_reservedCnt:\t" + ReservedCnt);
			sb.AppendLine("m_ghostRecCnt:\t" + GhostRecCnt);

			return sb.ToString();
		}
	}
}