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
			parseBody();
		}

		private void parseBody()
		{
			// Bytes 0-43 is the IAM page header

			// Read single page slot allocations
			Slot0 = new PageLocation(BitConverter.ToInt16(RawBody, 44), BitConverter.ToInt32(RawBody, 46));
			Slot1 = new PageLocation(BitConverter.ToInt16(RawBody, 50), BitConverter.ToInt32(RawBody, 52));
			Slot2 = new PageLocation(BitConverter.ToInt16(RawBody, 56), BitConverter.ToInt32(RawBody, 58));
			Slot3 = new PageLocation(BitConverter.ToInt16(RawBody, 62), BitConverter.ToInt32(RawBody, 64));
			Slot4 = new PageLocation(BitConverter.ToInt16(RawBody, 68), BitConverter.ToInt32(RawBody, 70));
			Slot5 = new PageLocation(BitConverter.ToInt16(RawBody, 74), BitConverter.ToInt32(RawBody, 76));
			Slot6 = new PageLocation(BitConverter.ToInt16(RawBody, 80), BitConverter.ToInt32(RawBody, 82));
			Slot7 = new PageLocation(BitConverter.ToInt16(RawBody, 86), BitConverter.ToInt32(RawBody, 88));

			// Byte 88 - 98 = ???

			// Rest is taken care of by ExtentAllocationMap
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