using System;
using System.Linq;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public class InternalLobSlotPointer : SlotPointer
	{
		public long Offset { get; private set; }

		/* Byte		Content
		 * 0-7		Offset (long)
		 * 8-11		PageID (int)
		 * 12-13	FileID (short)
		 * 14-15	SlotID (short)
		 */
		public InternalLobSlotPointer(byte[] bytes)
			: base(bytes.Skip(8).ToArray())
		{
			Offset = BitConverter.ToInt64(bytes, 0);
		}
	}
}