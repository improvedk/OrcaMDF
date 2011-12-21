using System;
using System.Linq;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	/* DATA (type: 3)
	 * Used to store data. Variable length size, in practice always > 64 + overhead bytes
	 * as it'll otherwise be stored in a SMALL_ROOT.
	 * 
	 * Byte		Content
	 * 0-7		Blob ID (long)
	 * 8-9		Type (short)
	 * 10-X		Data
	 */
	public class Data : LobStructureBase, ILobStructure
	{
		public long BlobID { get; private set; }
		private byte[] data;

		public Data(byte[] bytes, Database database)
			: base(database)
		{
			short type = BitConverter.ToInt16(bytes, 8);
			if (type != (short)LobStructureType.DATA)
				throw new ArgumentException("Invalid byte structure. Expected DATA, found " + type);

			BlobID = BitConverter.ToInt64(bytes, 0);
			data = bytes.Skip(10).ToArray();
		}

		public byte[] GetData()
		{
			return data;
		}
	}
}