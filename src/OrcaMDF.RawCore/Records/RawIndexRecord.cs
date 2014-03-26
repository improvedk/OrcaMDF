using OrcaMDF.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.RawCore.Records
{
	public class RawIndexRecord : RawRecord
	{
		public int? ChildPageID { get; private set; }
		public short? ChildFileID { get; private set; }

		private short pminlen;

		public RawIndexRecord(ArrayDelimiter<byte> bytes, short pminlen, byte level) : base(bytes)
		{
			this.pminlen = pminlen;
			
			// Fixed length size
			FixedLengthSize = (short)(pminlen - 1);

			// Fixed length data
			FixedLengthData = new ArrayDelimiter<byte>(bytes.SourceArray, bytes.Offset + 1, FixedLengthSize);

			// Null bitmap column count
			NullBitmapColumnCount = BitConverter.ToInt16(bytes.SourceArray, FixedLengthData.Offset + FixedLengthData.Count);

			// Null bitmap
			NullBitmapRawBytes = new ArrayDelimiter<byte>(bytes.SourceArray, FixedLengthData.Offset + FixedLengthData.Count + 2, (NullBitmapColumnCount + 7) / 8);
			NullBitmap = new BitArray(NullBitmapRawBytes.ToArray());

			// Variable length offset array
			if (HasVariableLengthColumns)
			{
				int endOfNullBitmapPointer = FixedLengthData.Offset + FixedLengthData.Count + 2 + NullBitmapRawBytes.Count;

				// Number of pointers
				NumberOfVariableLengthOffsetArrayEntries = BitConverter.ToInt16(bytes.SourceArray, bytes.Offset + endOfNullBitmapPointer);

				// Pointers
				VariableLengthOffsetArray = new List<short>();
				for (int i = 0; i < NumberOfVariableLengthOffsetArrayEntries; i++)
					VariableLengthOffsetArray.Add(BitConverter.ToInt16(bytes.SourceArray, bytes.Offset + endOfNullBitmapPointer + 2 + i * 2));

				// Values
				int endOfVariableLengthOffsetArrayPointer = endOfNullBitmapPointer + 2 + NumberOfVariableLengthOffsetArrayEntries.Value * 2;
				int previousPointer = endOfVariableLengthOffsetArrayPointer;

				VariableLengthOffsetValues = new List<ArrayDelimiter<byte>>();
				foreach (short entry in VariableLengthOffsetArray)
				{
					VariableLengthOffsetValues.Add(new ArrayDelimiter<byte>(bytes.SourceArray, bytes.Offset + previousPointer, entry - previousPointer));
					previousPointer = entry;
				}
			}

			// If we're at a non-leaf level, parse the page pointers
			if (level > 0)
			{
				ChildPageID = BitConverter.ToInt32(bytes.SourceArray, bytes.Offset + 1 + FixedLengthData.Count - 6);
				ChildFileID = BitConverter.ToInt16(bytes.SourceArray, bytes.Offset + 1 + FixedLengthData.Count - 2);
			}
		}
	}
}