using System;
using OrcaMDF.Core.Engine.Records.LobStructures.Exceptions;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public static class LobStructureFactory
	{
		public static ILobStructure Create(byte[] bytes, Database database)
		{
			// Type is stored in bytes 8-9 on all known lob structures
			short type = BitConverter.ToInt16(bytes, 8);
			LobStructureType lobType;

			if (Enum.IsDefined(typeof(LobStructureType), type))
				lobType = (LobStructureType)type;
			else
				throw new InvalidLobStructureType(type);

			switch(lobType)
			{
				case LobStructureType.SMALL_ROOT:
					return new SmallRoot(bytes, database);

				case LobStructureType.LARGE_ROOT_YUKON:
					return new LargeRootYukon(bytes, database);

				case LobStructureType.DATA:
					return new Data(bytes, database);

				case LobStructureType.INTERNAL:
					return new Internal(bytes, database);

				default:
					throw new InvalidLobStructureType(type);
			}
		}
	}
}