using System;
using System.Text;

namespace OrcaMDF.Core.Pages
{
	public class IamPage : ExtentAllocationMap
	{
		public PageLocation Slot0 { get; private set; }
		public PageLocation Slot1 { get; private set; }
		public PageLocation Slot2 { get; private set; }
		public PageLocation Slot3 { get; private set; }
		public PageLocation Slot4 { get; private set; }
		public PageLocation Slot5 { get; private set; }
		public PageLocation Slot6 { get; private set; }
		public PageLocation Slot7 { get; private set; }

		public IamPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseHeader();
		}

		private void parseHeader()
		{
			byte[] header = Records[0].FixedLengthData;

			// Read single page slot allocations
			Slot0 = new PageLocation(BitConverter.ToInt16(header, 46), BitConverter.ToInt32(header, 42));
			Slot1 = new PageLocation(BitConverter.ToInt16(header, 52), BitConverter.ToInt32(header, 48));
			Slot2 = new PageLocation(BitConverter.ToInt16(header, 58), BitConverter.ToInt32(header, 54));
			Slot3 = new PageLocation(BitConverter.ToInt16(header, 64), BitConverter.ToInt32(header, 60));
			Slot4 = new PageLocation(BitConverter.ToInt16(header, 70), BitConverter.ToInt32(header, 66));
			Slot5 = new PageLocation(BitConverter.ToInt16(header, 76), BitConverter.ToInt32(header, 72));
			Slot6 = new PageLocation(BitConverter.ToInt16(header, 82), BitConverter.ToInt32(header, 78));
			Slot7 = new PageLocation(BitConverter.ToInt16(header, 88), BitConverter.ToInt32(header, 84));
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("Slot0: " + Slot0);
			sb.AppendLine("Slot1: " + Slot1);
			sb.AppendLine("Slot2: " + Slot2);
			sb.AppendLine("Slot3: " + Slot3);
			sb.AppendLine("Slot4: " + Slot4);
			sb.AppendLine("Slot5: " + Slot5);
			sb.AppendLine("Slot6: " + Slot6);
			sb.AppendLine("Slot7: " + Slot7);
			sb.AppendLine();

			int currentRangeStartPageID = (PageID / 511232) * 511232;
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
	}
}