using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;
using OrcaMDF.Core.Engine.Records.LobStructures;

namespace OrcaMDF.Core.Engine.Records.VariableLengthDataProxies
{
	public class TextPointerProxy : DataProxy, IVariableLengthDataProxy
	{
		private byte[] bytes;
		private int timestamp;
		private SlotPointer lobRootSlot;

		public TextPointerProxy(Page page, byte[] bytes)
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
		
		public IEnumerable<byte> GetBytes()
		{
			// Get root lob structure bytes
			var rootLobStructurePage = OriginPage.Database.GetTextMixPage(lobRootSlot.PagePointer);
			var rootLobRecord = rootLobStructurePage.Records[lobRootSlot.SlotID];
			var rootLobStructure = LobStructureFactory.Create(rootLobRecord.FixedLengthData, OriginPage.Database);

			return rootLobStructure.GetData();
		}
	}
}