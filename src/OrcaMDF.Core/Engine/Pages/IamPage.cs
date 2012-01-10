using System;
using System.Collections.Generic;
using System.Text;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class IamPage : ExtentAllocationMap
	{
		public short Status { get; private set; }
		public byte PageCount { get; private set; }
		public short IndexID { get; private set; }
		public int ObjectID { get; private set; }
		public uint SequenceNumber { get; private set; }
		public PagePointer StartPage { get; private set; }
		public PagePointer Slot0 { get; private set; }
		public PagePointer Slot1 { get; private set; }
		public PagePointer Slot2 { get; private set; }
		public PagePointer Slot3 { get; private set; }
		public PagePointer Slot4 { get; private set; }
		public PagePointer Slot5 { get; private set; }
		public PagePointer Slot6 { get; private set; }
		public PagePointer Slot7 { get; private set; }

		public IamPage(byte[] bytes, Database database)
			: base(bytes, database)
		{
			parseHeader();
		}

		private void parseHeader()
		{
			/*
				Bytes	Content
				-----	-------
				00-03	SequenceNumber (int)
				04-13	?
				14-15	Status (smallint)
				16-27	?
				28-31	ObjectID (int)
				32-33	IndexID (smallint)
				34		PageCount (tinyint)
				35		?
				36-39	StartPage PageID (int)
				40-41	StartPage FileID (smallint)
				42-45	Slot0 PageID (int)
				46-47	Slot0 FileID (smallint)
				48-51	Slot1 PageID (int)
				52-53	Slot1 FileID (smallint)
				54-57	Slot2 PageID (v)
				58-59	Slot2 FileID (smallint)
				60-63	Slot3 PageID (int)
				64-65	Slot3 FileID (smallint)
				66-69	Slot4 PageID (int)
				70-71	Slot4 FileID (smallint)
				72-75	Slot5 PageID (int)
				76-77	Slot5 FileID (smallint)
				78-81	Slot6 PageID (int)
				82-83	Slot6 FileID (smallint)
				84-87	Slot7 PageID (int)
				88-89	Slot7 FileID (smallint)
			*/

			byte[] header = Records[0].FixedLengthData;

			// Read iam header
			SequenceNumber = BitConverter.ToUInt32(header, 0);
			Status = BitConverter.ToInt16(header, 14);
			ObjectID = BitConverter.ToInt32(header, 28);
			IndexID = BitConverter.ToInt16(header, 32);
			PageCount = header[34];
			StartPage = new PagePointer(BitConverter.ToInt16(header, 40), BitConverter.ToInt32(header, 36));

			// Read single page slot allocations
			Slot0 = new PagePointer(BitConverter.ToInt16(header, 46), BitConverter.ToInt32(header, 42));
			Slot1 = new PagePointer(BitConverter.ToInt16(header, 52), BitConverter.ToInt32(header, 48));
			Slot2 = new PagePointer(BitConverter.ToInt16(header, 58), BitConverter.ToInt32(header, 54));
			Slot3 = new PagePointer(BitConverter.ToInt16(header, 64), BitConverter.ToInt32(header, 60));
			Slot4 = new PagePointer(BitConverter.ToInt16(header, 70), BitConverter.ToInt32(header, 66));
			Slot5 = new PagePointer(BitConverter.ToInt16(header, 76), BitConverter.ToInt32(header, 72));
			Slot6 = new PagePointer(BitConverter.ToInt16(header, 82), BitConverter.ToInt32(header, 78));
			Slot7 = new PagePointer(BitConverter.ToInt16(header, 88), BitConverter.ToInt32(header, 84));
		}

		/// <summary>
		/// The IAM page references needs to use the FileID of the StartPage, as stored in the IAM page IAM header.
		/// </summary>
		public new IEnumerable<ExtentPointer> GetAllocatedExtents()
		{
			int gamRangeStartPageID = (Header.Pointer.PageID / 511232) * 511232;

			for (int i = 0; i < ExtentMap.Length; i++)
				if (ExtentMap[i])
					yield return new ExtentPointer(new PagePointer(StartPage.FileID, gamRangeStartPageID + i * 8));
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("SequenceNumber: " + SequenceNumber);
			sb.AppendLine("Status: " + Status);
			sb.AppendLine("ObjectID: " + ObjectID);
			sb.AppendLine("IndexID: " + IndexID);
			sb.AppendLine("PageCount: " + PageCount);
			sb.AppendLine("StartPage: " + StartPage);
			sb.AppendLine();
			sb.AppendLine("Slot0: " + Slot0);
			sb.AppendLine("Slot1: " + Slot1);
			sb.AppendLine("Slot2: " + Slot2);
			sb.AppendLine("Slot3: " + Slot3);
			sb.AppendLine("Slot4: " + Slot4);
			sb.AppendLine("Slot5: " + Slot5);
			sb.AppendLine("Slot6: " + Slot6);
			sb.AppendLine("Slot7: " + Slot7);
			sb.AppendLine();

			int currentRangeStartPageID = (Header.Pointer.PageID / 511232) * 511232;
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