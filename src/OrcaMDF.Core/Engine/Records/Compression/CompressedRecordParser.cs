using System;

namespace OrcaMDF.Core.Engine.Records.Compression
{
	internal class CompressedRecordParser
	{
		internal CompressedRecordFormat RecordFormat { get; private set; }
		internal bool HasVersioningInformation { get; private set; }
		internal CompressedRecordType RecordType { get; private set; }
		internal bool ContainsLongDataRegion { get; private set; }
		internal short NumberOfColumns { get; private set; }

		private readonly byte[] record;
		private short recordPointer;
		private CompressedRecordColumnCDIndicator[] columnValueIndicators;

		internal CompressedRecordParser(byte[] record)
		{
			this.record = record;

			parseHeader();
			parseCDRegion();
			parseShortDataRegion();
		}

		private void parseHeader()
		{
			byte header = record[0];

			// Bit 0
			RecordFormat = (header & 0x1) > 0 ? CompressedRecordFormat.CD : CompressedRecordFormat.Unknown;

			// Bit 1
			HasVersioningInformation = (header & 0x2) > 0;

			// Bits 2-4
			RecordType = (CompressedRecordType)((header << 3) >> 5);

			// Bit 5
			ContainsLongDataRegion = (header & 0x20) > 0;

			// Bits 6-7 unused in SQL Server 2008
		}

		private void parseCDRegion()
		{
			recordPointer = 1;

			// If the high order bit of the first byte is set, numColumns is a two-byte value,
			// otherwise it's a one-byte value.
			byte firstByte = record[recordPointer];
			if((firstByte & 0x80) > 0)
			{
				NumberOfColumns = BitConverter.ToInt16(record, 1);
				recordPointer += 2;
			}
			else
				NumberOfColumns = record[recordPointer++];

			// Next up we have 4 bits per column in the record. Loop all columns, alternating between reading
			// the first 4 bits, then the last 4 bits.
			columnValueIndicators = new CompressedRecordColumnCDIndicator[NumberOfColumns];
			for(int i=0; i<NumberOfColumns; i++)
				columnValueIndicators[i] = (CompressedRecordColumnCDIndicator)(i % 2 == 0 ? record[recordPointer] & 0xF : record[recordPointer++] & 0xF0);

			// Make sure to increase recordPointer if we end up reading the first 4 bits as the last
			// column, and thus need to pad up to nearest byte.
			if(NumberOfColumns % 2 == 0)
				recordPointer++;
		}

		private void parseShortDataRegion()
		{
			// Going to bed now, brb.
			throw new NotImplementedException();
		}
	}
}