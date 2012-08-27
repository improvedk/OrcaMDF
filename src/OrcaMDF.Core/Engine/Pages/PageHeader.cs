using System;
using System.Text;

namespace OrcaMDF.Core.Engine.Pages
{
	public class PageHeader
	{
		public short FreeCnt { get; private set; }
		public short FreeData { get; private set; }
		public short FlagBits { get; private set; }
		public string Lsn { get; private set; }
		public int ObjectID { get; private set; }
		public PageType Type { get; private set; }
		public short Pminlen { get; private set; }
		public short IndexID { get; private set; }
		public byte TypeFlagBits { get; private set; }
		public short SlotCnt { get; private set; }
		public string XdesID { get; private set; }
		public short XactReserved { get; private set; }
		public short ReservedCnt { get; private set; }
		public byte Level { get; private set; }
		public byte HeaderVersion { get; private set; }
		public short GhostRecCnt { get; private set; }
		public PagePointer NextPage { get; private set; }
		public PagePointer PreviousPage { get; private set; }
		public PagePointer Pointer { get; private set; }

		public PageHeader(byte[] header)
		{
			if (header.Length != 96)
				throw new ArgumentException("Header length must be 96.");

			/*
				Bytes	Content
				-----	-------
				00		HeaderVersion (tinyint)
				01		Type (tinyint)
				02		TypeFlagBits (tinyint)
				03		Level (tinyint)
				04-05	FlagBits (smallint)
				06-07	IndexID (smallint)
				08-11	PreviousPageID (int)
				12-13	PreviousFileID (smallint)
				14-15	Pminlen (smallint)
				16-19	NextPageID (int)
				20-21	NextPageFileID (smallint)
				22-23	SlotCnt (smallint)
				24-27	ObjectID (int)
				28-29	FreeCnt (smallint)
				30-31	FreeData (smallint)
				32-35	PageID (int)
				36-37	FileID (smallint)
				38-39	ReservedCnt (smallint)
				40-43	Lsn1 (int)
				44-47	Lsn2 (int)
				48-49	Lsn3 (smallint)
				50-51	XactReserved (smallint)
				52-55	XdesIDPart2 (int)
				56-57	XdesIDPart1 (smallint)
				58-59	GhostRecCnt (smallint)
				60-63	Checksum/Tornbits (int)
				64-95	?
			*/

			HeaderVersion = header[0];
			Type = (PageType)header[1];
			TypeFlagBits = header[2];
			Level = header[3];
			FlagBits = BitConverter.ToInt16(header, 4);
			IndexID = BitConverter.ToInt16(header, 6);
			PreviousPage = new PagePointer(BitConverter.ToInt16(header, 12), BitConverter.ToInt32(header, 8));
			Pminlen = BitConverter.ToInt16(header, 14);
			NextPage = new PagePointer(BitConverter.ToInt16(header, 20), BitConverter.ToInt32(header, 16));
			SlotCnt = BitConverter.ToInt16(header, 22);
			ObjectID = BitConverter.ToInt32(header, 24);
			FreeCnt = BitConverter.ToInt16(header, 28);
			FreeData = BitConverter.ToInt16(header, 30);
			Pointer = new PagePointer(BitConverter.ToInt16(header, 36), BitConverter.ToInt32(header, 32));
			ReservedCnt = BitConverter.ToInt16(header, 38);
			Lsn = "(" + BitConverter.ToInt32(header, 40) + ":" + BitConverter.ToInt32(header, 44) + ":" + BitConverter.ToInt16(header, 48) + ")";
			XactReserved = BitConverter.ToInt16(header, 50);
			XdesID = "(" + BitConverter.ToInt16(header, 56) + ":" + BitConverter.ToInt32(header, 52) + ")";
			GhostRecCnt = BitConverter.ToInt16(header, 58);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("m_freeCnt:\t" + FreeCnt);
			sb.AppendLine("m_freeData:\t" + FreeData);
			sb.AppendLine("m_flagBits:\t0x" + FlagBits.ToString("x"));
			sb.AppendLine("m_lsn:\t\t" + Lsn);
			sb.AppendLine("m_objId:\t" + ObjectID);
			sb.AppendLine("m_pageId:\t(" + Pointer.FileID + ":" + Pointer.PageID + ")");
			sb.AppendLine("m_type:\t\t" + Type);
			sb.AppendLine("m_typeFlagBits:\t" + "0x" + TypeFlagBits.ToString("x"));
			sb.AppendLine("pminlen:\t" + Pminlen);
			sb.AppendLine("m_indexId:\t" + IndexID);
			sb.AppendLine("m_slotCnt:\t" + SlotCnt);
			sb.AppendLine("m_nextPage:\t" + NextPage);
			sb.AppendLine("m_prevPage:\t" + PreviousPage);
			sb.AppendLine("m_xactReserved:\t" + XactReserved);
			sb.AppendLine("m_xdesId:\t" + XdesID);
			sb.AppendLine("m_reservedCnt:\t" + ReservedCnt);
			sb.AppendLine("m_ghostRecCnt:\t" + GhostRecCnt);

			return sb.ToString();
		}
	}
}