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

			/* 16 byte LOB pointer:
			 * 
			 * Bytes	Content
			 * 0-3		Timestamp (short)
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

			// DATA (type: 3)
			/* Byte		Content
			 * 0-1		Status bits
			 * 2-3		Fixed data length (short) - complete record length
			 * 4-7		Blob ID (int)
			 * 8-11		?
			 * 12-13	Type (short)
			 * 14-X		Data
			 */

			// SMALL_ROOT (type: 0)
			/* Byte		Content
			 * 0-1		Status bits
			 * 2-3		Fixed data length (short) - complete record length
			 * 4-7		Small blob ID (int)
			 * 8-11		?
			 * 12-13	Type (short)
			 * 14-19	?
			 * 20-X		Data
			 * X-?		?
			*/

			// LARGE_ROOT_YUKON (type: 5)
			/* Byte		Content
			 * 0-1		Status bits
			 * 2-3		Fixed data length (short) - complete record length
			 * 4-7		Blob ID (int)
			 * 8-11		?
			 * 12-13	Type (short)
			 * 14-15	MaxLinks (short)
			 * 16-23	?
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