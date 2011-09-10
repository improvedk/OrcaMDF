using System;
using System.Diagnostics;

namespace OrcaMDF.Core.Engine
{
	public class PagePointer
	{
		public short FileID;
		public int PageID;

		public static readonly PagePointer Zero = new PagePointer(0, 0);

		[DebuggerStepThrough]
		public PagePointer(short fileID, int pageID)
		{
			FileID = fileID;
			PageID = pageID;
		}

		public PagePointer(byte[] bytes)
		{
			if (bytes.Length != 6)
				throw new ArgumentException("Input must be 6 bytes in the format pageID(4)fileID(2).");

			PageID = BitConverter.ToInt32(bytes, 0);
			FileID = BitConverter.ToInt16(bytes, 4);
		}

		public static bool operator ==(PagePointer a, PagePointer b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(PagePointer a, PagePointer b)
		{
			return !a.Equals(b);
		}

		public override bool Equals(object obj)
		{
			var b = (PagePointer)obj;

			return b.FileID == FileID && b.PageID == PageID;
		}

		public override int GetHashCode()
		{
			// KISS
			return (FileID + "_" + PageID).GetHashCode();
		}

		[DebuggerStepThrough]
		public override string ToString()
		{
			return "(" + FileID + ":" + PageID + ")";
		}
	}
}