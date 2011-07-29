using System;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public static class LobStructureFactory
	{
		public ILobStructure Create(byte[] bytes)
		{
			short type = BitConverter.ToInt16(bytes, 8);

		}
	}
}