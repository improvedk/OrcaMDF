using System;
using System.Linq;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public class LobSlotPointer : SlotPointer
	{
		public int Size { get; private set; }

		/* Byte		Content
		 * 0-3		Size (int)
		 * 4-7		PageID (int)
		 * 8-9		FileID (short)
		 * 10-11	SlotID (short)
		 */
		public LobSlotPointer(byte[] bytes)
			: base(bytes.Skip(4).ToArray())
		{
			Size = BitConverter.ToInt32(bytes, 0);
		}
	}
}