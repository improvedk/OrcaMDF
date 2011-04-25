using System;
using System.Text;

namespace OrcaMDF.Core.Pages
{
	public class SgamPage : ExtentAllocationMap
	{
		public SgamPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{ }

		public override string ToString()
		{
			var sb = new StringBuilder();

			int currentRangeStartPageID = Header.PageID == 3 ? 0 : Header.PageID;
			int currentRangeStartMapIndex = 0;
			bool currentStatus = ExtentMap[0];
			for (int i = 0; i < ExtentMap.Length; i++)
			{
				if (ExtentMap[i] != currentStatus)
				{
					sb.AppendLine(currentRangeStartPageID + " - " + (currentRangeStartPageID + (i - currentRangeStartMapIndex - 1) * 8) + ": " + (currentStatus ? "ALLOCATED" : "NOT ALLOCATED"));
					
					// Start new range
					currentRangeStartPageID = currentRangeStartPageID + (i - currentRangeStartMapIndex) * 8;
					currentRangeStartMapIndex = i;
					currentStatus = !currentStatus;
				}
			}

			sb.AppendLine(currentRangeStartPageID + " - " + (currentRangeStartPageID + (ExtentMap.Length - currentRangeStartMapIndex - 1) * 8) + ": " + (currentStatus ? "ALLOCATED" : "NOT ALLOCATED"));

			return sb.ToString();
		}

		public static int GetSgamIndexByPageIndex(int index)
		{
			// First gam page is at index 3 and every 511232 pages hereafter
			return Math.Max(index / 511232 * 511232, 3);
		}
	}
}