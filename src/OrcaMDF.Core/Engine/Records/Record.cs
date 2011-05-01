using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Engine.Records
{
	public abstract class Record
	{
		protected Page Page;

		public RecordType Type { get; protected set; }
		public bool HasNullBitmap { get; protected set; }
		public bool HasVariableLengthColumns { get; protected set; }
		public byte[] FixedLengthData { get; protected set; }
		public short NumberOfColumns { get; protected set; }
		public BitArray NullBitmap { get; protected set; }
		public short NumberOfVariableLengthColumns { get; protected set; }
		public IDictionary<int, byte[]> VariableLengthColumnData { get; protected set; }

		protected Record(Page page)
		{
			Page = page;
		}

		protected void ParseVariableLengthColumns(byte[] bytes, ref short offset)
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
					VariableLengthColumnData[i] = GetOverflowDataFromPointer(VariableLengthColumnData[i]);
			}
		}

		protected byte[] GetOverflowDataFromPointer(byte[] data)
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
				var textMixPage = Page.File.GetTextMixPage(new PagePointer(fileID, pageID));
				fieldData = fieldData.Concat(textMixPage.Records[slot].FixedLengthData).ToArray();
			}
			
			return fieldData;
		}

		protected short ParseNullBitmap(byte[] bytes, ref short offset)
		{
			NullBitmap = new BitArray(bytes.Skip(offset).Take((NumberOfColumns + 7)/8).ToArray());
			offset += (short)((NumberOfColumns + 7) / 8);
			return offset;
		}
	}
}