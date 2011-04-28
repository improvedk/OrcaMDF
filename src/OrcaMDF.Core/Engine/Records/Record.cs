using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.Core.Engine.Records
{
	public class Record
	{
		private MdfFile file;

		public RecordType Type { get; private set; }
		public bool HasNullBitmap { get; private set; }
		public bool HasVariableLengthColumns { get; private set; }
		public bool HasVersioningInformation { get; private set; }
		public bool IsGhostForwardedRecord { get; private set; }
		public byte[] FixedLengthData { get; private set; }
		public short NumberOfColumns { get; private set; }
		public BitArray NullBitmap { get; private set; }
		public short NumberOfVariableLengthColumns { get; private set; }
		public IDictionary<int, byte[]> VariableLengthColumnData { get; private set; }

		public Record(byte[] bytes, MdfFile file)
		{
			this.file = file;
			short offset = 0;
			
			parseStatusBitsA(new BitArray(new [] { bytes[offset++] }));
			parseStatusBitsB(bytes[offset++]);

			short fixedLengthSize = BitConverter.ToInt16(bytes, offset);
			fixedLengthSize -= 4;
			offset += 2;

			switch(Type)
			{
				case RecordType.Forwarded:
					// Ignore 10 byte forwarded record header
					offset += 10;
					fixedLengthSize -= 10;

					FixedLengthData = bytes.Skip(offset).Take(fixedLengthSize).ToArray();
					offset += fixedLengthSize;
					break;

				default:
					FixedLengthData = bytes.Skip(offset).Take(fixedLengthSize).ToArray();
					offset += fixedLengthSize;
					break;
			}

			NumberOfColumns = BitConverter.ToInt16(bytes, offset);
			offset += 2;

			if (HasNullBitmap)
				offset = parseNullBitmap(bytes, ref offset);

			if (HasVariableLengthColumns)
				parseVariableLengthColumns(bytes, ref offset);
		}

		private void parseVariableLengthColumns(byte[] bytes, ref short offset)
		{
			NumberOfVariableLengthColumns = BitConverter.ToInt16(bytes, offset);
			offset += 2;

			short[] variableLengthColumnLengths = new short[NumberOfVariableLengthColumns];
			for (int i = 0; i < NumberOfVariableLengthColumns; i++)
			{
				variableLengthColumnLengths[i] = BitConverter.ToInt16(bytes, offset);
				offset += 2;
			}

			VariableLengthColumnData = new Dictionary<int, byte[]>();
			for(int i=0; i<NumberOfVariableLengthColumns; i++)
			{
				bool overflowData = false;

				// The high order sign bit is used to indicate whether data is stored off-row (http://bit.ly/gtFC8P)
				if ((variableLengthColumnLengths[i] & 32768) == 32768)
				{
					// Strip the sign bit and remember that we're dealing with row overflow data
					variableLengthColumnLengths[i] = (short)(variableLengthColumnLengths[i] & Int16.MaxValue);
					overflowData = true;
				}

				VariableLengthColumnData[i] = bytes.Skip(offset).Take(variableLengthColumnLengths[i] - offset).ToArray();
				offset = variableLengthColumnLengths[i];

				if (overflowData)
					VariableLengthColumnData[i] = getOverflowDataFromPointer(VariableLengthColumnData[i]);
			}
		}

		private byte[] getOverflowDataFromPointer(byte[] data)
		{
			byte spcialFieldType = data[0];
			short indexLevel = BitConverter.ToInt16(data, 1);
			int sequence = BitConverter.ToInt32(data, 4);
			int timestamp = BitConverter.ToInt32(data, 8);

			byte[] fieldData = new byte[0];
			for(int i=12; i<data.Length; i += 12)
			{
				int length = BitConverter.ToInt32(data, i);
				int pageID = BitConverter.ToInt32(data, i + 4);
				short fileID = BitConverter.ToInt16(data, i + 8);
				short slot = BitConverter.ToInt16(data, i + 10);

				// Get referenced page
				var textMixPage = file.GetTextMixPage(pageID);
				fieldData = fieldData.Concat(textMixPage.Records[slot].FixedLengthData).ToArray();
			}
			
			return fieldData;
		}

		private short parseNullBitmap(byte[] bytes, ref short offset)
		{
			NullBitmap = new BitArray(bytes.Skip(offset).Take((NumberOfColumns + 7)/8).ToArray());
			offset += (short)((NumberOfColumns + 7) / 8);
			return offset;
		}

		private void parseStatusBitsA(BitArray bits)
		{
			// Bit 0 (versioning bit) we don't care about as it's always 0 in 2k8+

			// Bits 1-3 represents record type
			Type = (RecordType)((Convert.ToByte(bits[1]) << 2) + (Convert.ToByte(bits[2]) << 1) + Convert.ToByte(bits[3]));
			
			// Bit 4 determines whether a null bitmap is present
			HasNullBitmap = bits[4];

			// Bit 5 determines whether there are variable length columns
			HasVariableLengthColumns = bits[5];

			// Bit 6 determines whether the row contains versioning information
			HasVersioningInformation = bits[6];

			// Bit 7 isn't used in 2k8+
		}

		private void parseStatusBitsB(byte bits)
		{
			IsGhostForwardedRecord = bits == 1;
		}

		public void WriteHeaderToConsole()
		{
			Console.WriteLine("Type:".PadRight(35) + Type);
			Console.WriteLine("HasNullBitmap:".PadRight(35) + HasNullBitmap);
			Console.WriteLine("HasVariableLengthColumns:".PadRight(35) + HasVariableLengthColumns);
			Console.WriteLine("NumberOfVariableLengthColumns:".PadRight(35) + NumberOfVariableLengthColumns);
			Console.WriteLine("HasVersioningInformation:".PadRight(35) + HasVersioningInformation);
			Console.WriteLine("IsGhostForwardedRecord:".PadRight(35) + IsGhostForwardedRecord);
			Console.WriteLine("NumberOfColumns:".PadRight(35) + NumberOfColumns);
			Console.WriteLine("FixedLengthData:".PadRight(35) + "byte[" + FixedLengthData.Length + "]");

			if (HasNullBitmap)
			{
				Console.Write("NullBitmap:".PadRight(35) + "{");
				for (int x = 0; x < NullBitmap.Length; x++)
					Console.Write(Convert.ToInt16(NullBitmap[x]));
				Console.WriteLine("}");
			}
		}
	}
}