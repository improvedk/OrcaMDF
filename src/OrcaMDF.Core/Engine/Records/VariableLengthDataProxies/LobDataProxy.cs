using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;
using OrcaMDF.Core.Engine.Records.LobStructures;

namespace OrcaMDF.Core.Engine.Records.VariableLengthDataProxies
{
	public class LobDataProxy : DataProxy, IVariableLengthDataProxy
	{
		private byte[] bytes;
		private int timestamp;
		private SlotPointer lobRootSlot;

		public LobDataProxy(Page page, byte[] bytes)
			: base(page)
		{
			this.bytes = bytes;

			/* 16 byte LOB Textpointer:
			 * 
			 * Bytes	Content
			 * 0-3		Timestamp (int)
			 * 4-7		?
			 * 8-16		Slot pointer
			*/

			timestamp = BitConverter.ToInt32(bytes, 0);
			lobRootSlot = new SlotPointer(bytes.Skip(8).ToArray());
		}

		/* SMALL_ROOT (type: 0)
		 * Used whenever the inline data length is <= 64. Fixed size of 84 bytes.
		 * 
		 * Byte		Content
		 * 0-1		Status bits
		 * 2-3		Fixed data length (short) - complete record length
		 * 4-11		Small blob ID (long)
		 * 12-13	Type (short)
		 * 14-15	Length (short)
		 * 16-19	?
		 * 20-83	Data (everything above [Length] is to be considered garbage). Max SMALL_ROOT data storage = 64 bytes
		 */

		/* LARGE_ROOT (type: 1)
		 * ? No idea
		 */

		/* INTERNAL (type: 2)
		 * ? No idea
		 */

		/* DATA (type: 3)
		 * Used to store data. Variable length size, in practice always > 64 + overhead bytes
		 * as it'll otherwise be stored in a SMALL_ROOT.
		 * 
		 * Byte		Content
		 * 0-1		Status bits
		 * 2-3		Fixed data length (short) - complete record length
		 * 4-11		Blob ID (long)
		 * 12-13	Type (short)
		 * 14-X		Data
		 */

		/* LARGE_ROOT_SHILOH (type: 4)
		 * Used in SQL Server 2000 (code name SHILOH)
		 */

		/* LARGE_ROOT_YUKON (type: 5)
		 * Used to store a range of page references, either directly to data pages or as part
		 * of a b-tree structure referencing other LARGE_ROOT_YUKON structures.
		 * 
		 * Byte		Content
		 * 0-1		Status bits
		 * 2-3		Fixed data length (short) - complete record length
		 * 4-11		Blob ID (long)
		 * 12-13	Type (short)
		 * 14-15	MaxLinks (short)
		 * 16-17	CurLinks (short)
		 * 18-19	Level (short)
		 * 20-23	?
		 * 24-27	Offset[0] (int) - This'll be the *end* of the data. For [0] size == offset. For [n] size == [n]-[n-1].
		 * 28-31	PageID[0] (int)
		 * 32-33	FileID[0] (short)
		 * 34-35	SlotID[0] (short)
		 * 36-39	Offset[1] (int)
		 * 40-43	PageID[1] (int)
		 * 44-45	FileID[1] (short)
		 * 46-47	SlotID[1] (short)
		 */

		/* SUPER_LARGE_ROOT (type: 6)
		 * ? No idea
		 */

		/* (type: 7)
		 * Doesn't show type name, probably unused
		 */

		/* NULL (type: 8)
		 * ? No idea
	 	 */

		/* INVALID (type: 9)
		 * ? Probably invalid
		 */

		/* INVALID (type: 10)
		 * ? I should probably stop now...
		 */

		public IEnumerable<byte> GetBytes()
		{
			/* Choosing this way of parsing feels a bit wrong. Ideally I would treat all text structures
			 * as normal records, as that's what they are. That means I should be subtyping PrimaryRecord.
			 * However, there are some differences, e.g. these are technically forwarded records, yet they
			 * don't contain a forwarded record header. As such, I'd need to split up PrimaryRecord even more.
			 * Instead I'm keeping this in a tight parsing schema made explicitly for parsing the LOB text structures.
			 * Speed, simplicity and readability wins over the 'right' design way of doing it, for now. Sorry.
			 */

			// Get root lob structure bytes
			var rootLobStructurePage = OriginPage.File.GetTextMixPage(lobRootSlot.PagePointer);
			var rootLobRecord = rootLobStructurePage.Records[lobRootSlot.SlotID];

			switch(rootLobRecord.LobType)
			{
				case LobStructureType.SMALL_ROOT:
					var smallRoot = new SmallRoot(rootLobRecord.FixedLengthData, OriginPage.File);
					return smallRoot.GetData();

				case LobStructureType.LARGE_ROOT_YUKON:
					var largeRootYukon = new LargeRootYukon(rootLobRecord.FixedLengthData, OriginPage.File);
					return largeRootYukon.GetData();

				default:
					throw new ArgumentException("Unsupported LOB structure encountered: " + rootLobRecord.LobType);
			}
		}
	}
}