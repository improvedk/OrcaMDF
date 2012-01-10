using System;
using System.Text;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class GamPage : ExtentAllocationMap
	{
		public GamPage(byte[] bytes, Database database)
			: base(bytes, database)
		{ }

		public override string ToString()
		{
			var sb = new StringBuilder();

			int currentRangeStartPageID = Header.Pointer.PageID == 2 ? 0 : Header.Pointer.PageID;
			int currentRangeStartMapIndex = 0;
			bool currentStatus = ExtentMap[0];
			for (int i = 0; i < ExtentMap.Length; i++)
			{
				if (ExtentMap[i] != currentStatus)
				{
					sb.AppendLine(currentRangeStartPageID + " - " + (currentRangeStartPageID + (i - currentRangeStartMapIndex - 1) * 8) + ": " + (currentStatus ? "NOT ALLOCATED" : "ALLOCATED"));
					
					// Start new range
					currentRangeStartPageID = currentRangeStartPageID + (i - currentRangeStartMapIndex) * 8;
					currentRangeStartMapIndex = i;
					currentStatus = !currentStatus;
				}
			}

			sb.AppendLine(currentRangeStartPageID + " - " + (currentRangeStartPageID + (ExtentMap.Length - currentRangeStartMapIndex - 1) * 8) + ": " + (currentStatus ? "NOT ALLOCATED" : "ALLOCATED"));

			return sb.ToString();
		}
		
		public static PagePointer GetGamPointerForPage(PagePointer loc)
		{
			// First gam page is at index 2 and every 511232 pages hereafter
			return new PagePointer(loc.FileID, Math.Max(loc.PageID / 511232 * 511232, 2));
		}
	}
}