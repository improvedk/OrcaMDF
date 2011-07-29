namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public enum LobStructureType : short
	{
		SMALL_ROOT			= 0,
		LARGE_ROOT			= 1,
		INTERNAL			= 2,
		DATA				= 3,
		LARGE_ROOT_SHILOH	= 4,
		LARGE_ROOT_YUKON	= 5,
		SUPER_LARGE_ROOT	= 6,
		// 7 Seems to exist but doesn't have a name. Unaware of it's usage.
		NULL				= 8
		// 9+ are all INVALID AFAIK.
	}
}