using System;

namespace OrcaMDF.Core.Engine.Pages.PFS
{
	[Flags]
	public enum PfsFlags : byte
	{
		Empty			= 0x0,
		UpTo50			= 0x1,
		UpTo80			= 0x2,
		UpTo95			= 0x3,
		UpTo100			= 0x4,
		GhostRecords	= 0x8,
		IAM				= 0x10,
		MixedExtent		= 0x20,
		Allocated		= 0x40
	}
}