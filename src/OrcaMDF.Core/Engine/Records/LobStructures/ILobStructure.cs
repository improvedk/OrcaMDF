using System.Collections.Generic;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public interface ILobStructure
	{
		IEnumerable<byte> GetData();
	}
}