using System;
using OrcaMDF.Core.Engine.Records.LobStructures.Exceptions;

namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public static class LobStructureFactory
	{
		public static ILobStructure Create(byte[] bytes, MdfFile file)
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
					return new SmallRootLobStructure(bytes, file);

				case LobStructureType.LARGE_ROOT_YUKON:
					return new LargeRootYukonLobStructure(bytes, file);

				case LobStructureType.DATA:
					return new DataLobStructure(bytes, file);

				case LobStructureType.INTERNAL:
					return new InternalLobStructure(bytes, file);

				default:
					throw new InvalidLobStructureType(type);
			}
		}
	}
}