using System;
using System.Text;

namespace Orca.MdfReader.Adhoc.Pages
{
	public class GamPage : ExtentAllocationMap
	{
		public GamPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{ }

		public override string ToString()
		{
			var sb = new StringBuilder();

			int currentRangeStartPageID = this.PageID == 2 ? 0 : this.PageID;
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

		public static int GetGamIndexByPageIndex(int index)
		{
			// First gam page is at index 2 and every 511232 pages hereafter
			return Math.Max(index / 511232 * 511232, 2);
		}
	}
}