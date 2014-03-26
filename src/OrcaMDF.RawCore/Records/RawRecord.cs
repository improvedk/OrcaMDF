using OrcaMDF.Framework;
using System.Collections;
using System.Collections.Generic;

namespace OrcaMDF.RawCore.Records
{
	public class RawRecord
	{
		public bool Version { get; private set; }
		public RecordType Type { get; private set; }
		public bool HasNullBitmap { get; private set; }
		public bool HasVariableLengthColumns { get; private set; }
		public bool HasVersioningInformation { get; private set; }
		public short FixedLengthSize { get; protected set; }
		public ArrayDelimiter<byte> FixedLengthData { get; protected set; }
		public short NullBitmapColumnCount { get; protected set; }
		public BitArray NullBitmap { get; protected set; }
		public short? NumberOfVariableLengthOffsetArrayEntries { get; protected set; }
		public List<short> VariableLengthOffsetArray { get; protected set; }
		public List<ArrayDelimiter<byte>> VariableLengthOffsetValues { get; protected set; }

		internal byte RawStatusByteA { get; private set; }
		internal ArrayDelimiter<byte> NullBitmapRawBytes { get; set; }

		internal RawRecord(ArrayDelimiter<byte> bytes)
		{
			// Status byte A
			RawStatusByteA = bytes[0];
			Version = (RawStatusByteA & 0x01) == 0x01; // First bit
			Type = (RecordType)((RawStatusByteA & 0x0E) >> 1); // 2-4th bits
			HasNullBitmap = (RawStatusByteA & 0x10) == 0x10; // Fifth bit
			HasVariableLengthColumns = (RawStatusByteA & 0x20) == 0x20; // Sixth bit
			HasVersioningInformation = (RawStatusByteA & 0x40) == 0x40; // Seventh bit
		}
	}
}