using OrcaMDF.Framework;
using System;

namespace OrcaMDF.RawCore
{
	public class RawPageHeader
	{
		private readonly RawPage page;

		public ArraySegment<byte> RawBytes
		{
			get { return new ArraySegment<byte>(page.RawBytes, 0, 96); }
		}

		public short FreeCnt
		{
			get { return BitConverter.ToInt16(page.RawBytes, 28); }
		}

		public short FreeData
		{
			get { return BitConverter.ToInt16(page.RawBytes, 30); }
		}

		public ushort FlagBits
		{
			get { return BitConverter.ToUInt16(page.RawBytes, 4); }
		}

		public string Lsn
		{
			get { return "(" + BitConverter.ToInt32(page.RawBytes, 40) + ":" + BitConverter.ToInt32(page.RawBytes, 44) + ":" + BitConverter.ToInt16(page.RawBytes, 48) + ")"; }
		}

		public int ObjectID
		{
			get { return BitConverter.ToInt32(page.RawBytes, 24); }
		}

		public PageType Type
		{
			get { return (PageType)page.RawBytes[1]; }
		}

		public short Pminlen
		{
			get { return BitConverter.ToInt16(page.RawBytes, 14); }
		}

		public short IndexID
		{
			get { return BitConverter.ToInt16(page.RawBytes, 6); }
		}

		public byte TypeFlagBits
		{
			get { return page.RawBytes[2]; }
		}

		public short SlotCnt
		{
			get { return BitConverter.ToInt16(page.RawBytes, 22); }
		}

		public string XdesID
		{
			get { return "(" + BitConverter.ToInt16(page.RawBytes, 56) + ":" + BitConverter.ToInt32(page.RawBytes, 52) + ")"; }
		}

		public short XactReserved
		{
			get { return BitConverter.ToInt16(page.RawBytes, 50); }
		}

		public short ReservedCnt
		{
			get { return BitConverter.ToInt16(page.RawBytes, 38); }
		}

		public byte Level
		{
			get { return page.RawBytes[3]; }
		}

		public byte HeaderVersion
		{
			get { return page.RawBytes[0]; }
		}

		public short GhostRecCnt
		{
			get { return BitConverter.ToInt16(page.RawBytes, 58); }
		}

		public short NextPageFileID
		{
			get { return BitConverter.ToInt16(page.RawBytes, 20); }
		}

		public int NextPageID
		{
			get { return BitConverter.ToInt32(page.RawBytes, 16); }
		}

		public short PreviousPageFileID
		{
			get { return BitConverter.ToInt16(page.RawBytes, 12); }
		}

		public int PreviousPageID
		{
			get { return BitConverter.ToInt32(page.RawBytes, 8); }
		}

		public int PageID
		{
			get { return BitConverter.ToInt32(page.RawBytes, 32); }
		}

		public short FileID
		{
			get { return BitConverter.ToInt16(page.RawBytes, 36); }
		}

		public ArraySegment<byte> Checksum
		{
			get { return new ArraySegment<byte>(page.RawBytes, 60, 4); }
		}

		public RawPageHeader(RawPage page)
		{
			this.page = page;
		}
	}
}