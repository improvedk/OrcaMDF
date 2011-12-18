namespace OrcaMDF.Core.Engine.Records.Compression
{
	internal enum CompressedRecordColumnCDIndicator : byte
	{
		Null				= 0x0,
		ZeroByte			= 0x1,
		OneByte				= 0x2,
		TwoByte				= 0x3,
		ThreeByte			= 0x4,
		FourByte			= 0x5,
		FiveByte			= 0x6,
		SixByte				= 0x7,
		SevenByte			= 0x8,
		EightByte			= 0x9,
		LongData			= 0xA,
		TrueBit				= 0xB,
		DictionarySymbol	= 0xC
	}
}