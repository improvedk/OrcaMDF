using System;

namespace OrcaMDF.Core.MetaData.SystemFlags
{
	// Values extracted from http://www.g-productions.nl/index.php?name=system_internals_partitions&version=2008SP2
	[Flags]
	public enum RowsetStatus
	{
		RS_LOBSTAT				= 3,
		RS_UNIQUE				= 4,
		RS_REPLICATED			= 8,
		RS_LOG_OFFROW_FOR_REPL	= 16,
		RS_NULLABLE_KEYS		= 64,
		RS_NO_ROWLOCK			= 128,
		RS_NO_PAGELOCK			= 256,
		RS_DATAROW_FRMT			= 512,
		RS_NOTVERSIONED			= 2048
	}
}