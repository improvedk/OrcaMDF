using System;

namespace OrcaMDF.Core.Engine
{
	public class PageLocation
	{
		public short FileID;
		public int PageID;

		public PageLocation(short fileID, int pageID)
		{
			this.FileID = fileID;
			this.PageID = pageID;
		}

		public PageLocation(byte[] bytes)
		{
			if (bytes.Length != 6)
				throw new ArgumentException("Input must be 6 bytes in the format pageID(4)fileID(2).");

			PageID = BitConverter.ToInt32(bytes, 0);
			FileID = BitConverter.ToInt16(bytes, 4);
		}

		public override string ToString()
		{
			return "(" + FileID + ":" + PageID + ")";
		}
	}
}