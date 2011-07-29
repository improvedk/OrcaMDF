using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	/* SMALL_ROOT (type: 0)
	 * Used whenever the inline data length is <= 64. Fixed size of 84 bytes, including record overhead.
	 * 
	 * Byte		Content
	 * 0-7		Small blob ID (long)
	 * 8-9		Type (short)
	 * 10-11	Length (short)
	 * 12-15	?
	 * 16-79	Data (everything above [Length] is to be considered garbage). Max SMALL_ROOT data storage = 64 bytes
	 */
	public class SmallRoot : BaseLobStructure, ILobStructure
	{
		public long BlobID { get; private set; }
		public short Length { get; private set; }
		private byte[] data;

		public SmallRoot(byte[] bytes, MdfFile file)
			: base(file)
		{
			short type = BitConverter.ToInt16(bytes, 8);
			if(type != (short)LobStructureType.SMALL_ROOT)
				throw new ArgumentException("Invalid byte structure. Expected SMALL_ROOT, found " + type);
			
			BlobID = BitConverter.ToInt64(bytes, 0);
			Length = BitConverter.ToInt16(bytes, 10);
			data = bytes.Skip(16).Take(Length).ToArray();
		}

		public IEnumerable<byte> GetData()
		{
			return data;
		}
	}
}