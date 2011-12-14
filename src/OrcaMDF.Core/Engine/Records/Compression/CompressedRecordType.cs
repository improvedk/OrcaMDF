namespace OrcaMDF.Core.Engine.Records.Compression
{
	internal enum CompressedRecordType : byte
	{
		Primary			= 0,
		GhostEmpty		= 1,
		Forwarding		= 2,
		GhostData		= 3,
		Forwarded		= 4,
		GhostForwarded	= 5,
		Index			= 6,
		GhostIndex		= 7
	}
}