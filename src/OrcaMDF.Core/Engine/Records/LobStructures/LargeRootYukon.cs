using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	/* LARGE_ROOT_YUKON (type: 5)
	 * Used to store a range of page references, either directly to data pages or as part
	 * of a b-tree structure referencing other LARGE_ROOT_YUKON structures.
	 * 
	 * Byte		Content
	 * 0-7		Blob ID (long)
	 * 8-9		Type (short)
	 * 10-11	MaxLinks (short)
	 * 12-13	CurLinks (short) - Number of Slot Pointers present in the LARGE_ROOT_YUKON structure
	 * 14-15	Level (short)
	 * 16-19	?
	 * 20-23	Size[0] (int) - This'll be the *end* of the data. For [0] size == offset. For [n] size == [n]-[n-1].
	 * 24-27	PageID[0] (int)
	 * 28-29	FileID[0] (short)
	 * 30-31	SlotID[0] (short)
	 * ...
	 */
	public class LargeRootYukon : LobStructureBase, ILobStructure
	{
		public long BlobID { get; private set; }
		public short MaxLinks { get; private set; }
		public short CurLinks { get; private set; }
		public short Level { get; private set; }
		public LobSlotPointer[] DataSlotPointers { get; private set; }

		public LargeRootYukon(byte[] bytes, Database database)
			: base(database)
		{
			short type = BitConverter.ToInt16(bytes, 8);
			if(type != (short)LobStructureType.LARGE_ROOT_YUKON)
				throw new ArgumentException("Invalid byte structure. Expected LARGE_ROOT_YUKON, found " + type);
			
			BlobID = BitConverter.ToInt64(bytes, 0);
			MaxLinks = BitConverter.ToInt16(bytes, 10);
			CurLinks = BitConverter.ToInt16(bytes, 12);
			Level = BitConverter.ToInt16(bytes, 14);
			DataSlotPointers = new LobSlotPointer[CurLinks];

			short offset = 20;
			for(short i=0; i<CurLinks; i++)
			{
				DataSlotPointers[i] = new LobSlotPointer(bytes.Skip(offset).Take(12).ToArray());
				offset += 12;
			}
		}

		public byte[] GetData()
		{
			var result = new List<byte>();

			foreach(var lobSlot in DataSlotPointers)
			{
				var textPage = Database.GetTextMixPage(lobSlot.PagePointer);
				var lobRecord = textPage.Records[lobSlot.SlotID];
				var lobStructure = LobStructureFactory.Create(lobRecord.FixedLengthData, Database);

				result.AddRange(lobStructure.GetData());
			}

			return result.ToArray();
		}
	}
}