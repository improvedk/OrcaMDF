using System;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Engine.Records.VariableLengthDataProxies
{
	public class LobDataProxy : DataProxy, IVariableLengthDataProxy
	{
		private byte[] data;
		private int timestamp;
		private SlotPointer slotPointer;

		public LobDataProxy(Page page, byte[] data)
			: base(page)
		{
			this.data = data;

			/* 16 byte LOB Textpointer:
			 * 
			 * Bytes	Content
			 * 0-3		Timestamp (int)
			 * 4-7		?
			 * 8-16		Slot pointer
			*/
			timestamp = BitConverter.ToInt32(data, 0);
			slotPointer = new SlotPointer(data.Skip(8).ToArray());
		}

		public byte[] GetBytes()
		{
			var textMixPage = OriginPage.File.GetTextMixPage(new PagePointer(slotPointer.FileID, slotPointer.PageID));
			var slot = textMixPage.Records[slotPointer.SlotID];

			/* SMALL_ROOT (type: 0)
			 * Used whenever the inline data length is <= 64. Fixed size of 84 bytes.
			 * 
			 * Byte		Content
			 * 0-1		Status bits
			 * 2-3		Fixed data length (short) - complete record length
			 * 4-7		Small blob ID (int)
			 * 8-11		? (All observations so far == 0)
			 * 12-13	Type (short)
			 * 14-15	Length (short)
			 * 16-19	?
			 * 20-83	Data (everything above [Length] is to be considered garbage). Max SMALL_ROOT data storage = 64 bytes
			*/

			/* DATA (type: 3)
			 * Used to store data. Variable length size, in practice always > 64 + overhead bytes
			 * as it'll otherwise be stored in a SMALL_ROOT.
			 * 
			 * TODO: Test what happens if LARGE_ROOT_YUKON contains just enough data to point to
			 * a normal data page + <= 64 bytes of extra data. Will it combine DATA reference + SMALL_ROOT or
			 * just use two data pages?
			 * 
			 * Byte		Content
			 * 0-1		Status bits
			 * 2-3		Fixed data length (short) - complete record length
			 * 4-7		Blob ID (int)
			 * 8-11		? (All oservations so far == 0)
			 * 12-13	Type (short)
			 * 14-X		Data
			 */

			/* LARGE_ROOT_YUKON (type: 5)
			 * Used to store a range of page references, either directly to data pages or as part
			 * of a b-tree structure referencing other LARGE_ROOT_YUKON structures.
			 * 
			 * Byte		Content
			 * 0-1		Status bits
			 * 2-3		Fixed data length (short) - complete record length
			 * 4-7		Blob ID (int)
			 * 8-11		? (All observations so far == 0)
			 * 12-13	Type (short)
			 * 14-15	MaxLinks (short) - TODO: Verify
			 * 16-23	? - Probably contains CurLinks + Level + unknown - TODO: Verify
			 * 24-25	Size (short)
			 * 26-27	?
			 * 28-31	PageID (int)
			 * 32-33	FileID (short)
			 * 34-35	SlotID (short)
			 * 36-?		?
			*/

			return data;
		}
	}
}