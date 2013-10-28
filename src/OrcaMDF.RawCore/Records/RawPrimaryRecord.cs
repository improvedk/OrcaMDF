using System;
using System.Collections.Generic;

namespace OrcaMDF.RawCore.Records
{
	public class RawPrimaryRecord : RawRecord
	{
		private short? _pointerToEndOfNullBitmap;
		private short pointerToEndOfNullBitmap
		{
			get
			{
				if (_pointerToEndOfNullBitmap == null)
					_pointerToEndOfNullBitmap = (short)(NullBitmapPointer + 2 + ((NullBitmapColumnCount + 7) / 8));

				return _pointerToEndOfNullBitmap.Value;
			}
		}

		public byte RawStatusByteB
		{
			get { return DB.Data[Page.FileID][Index + 1]; }
		}

		public bool IsGhostForwardedRecord
		{
			get { return RawStatusByteB == 1; }
		}

		public short NullBitmapPointer
		{
			get { return BitConverter.ToInt16(DB.Data[Page.FileID], Index + 2); }
		}

		public short NullBitmapColumnCount
		{
			get { return BitConverter.ToInt16(DB.Data[Page.FileID], Index + NullBitmapPointer); }
		}

		public ArraySegment<byte> FixedLengthData
		{
			get { return new ArraySegment<byte>(DB.Data[Page.FileID], Index + 4, NullBitmapPointer - 4); }
		}

		public ArraySegment<byte> NullBitmapRawBytes
		{
			get { return new ArraySegment<byte>(DB.Data[Page.FileID], Index + NullBitmapPointer + 2, (NullBitmapColumnCount + 7) / 8); }
		}

		public short? NumberOfVariableLengthOffsetArrayEntries
		{
			get
			{
				if (!HasVariableLengthColumns)
					return null;

				return BitConverter.ToInt16(DB.Data[Page.FileID], Index + pointerToEndOfNullBitmap);
			}
		}

		public IEnumerable<short> VariableLengthOffsetArray
		{
			get
			{
				if (HasVariableLengthColumns)
				{
					for (int i = 0; i < NumberOfVariableLengthOffsetArrayEntries; i++)
						yield return BitConverter.ToInt16(DB.Data[Page.FileID], Index + pointerToEndOfNullBitmap + 2 + i * 2);
				}
			}
		}

		public IEnumerable<ArraySegment<byte>> VariableLengthOffsetValues
		{
			get
			{
				if (HasVariableLengthColumns)
				{
					int endOfVariableLengthArray = pointerToEndOfNullBitmap + 2 + NumberOfVariableLengthOffsetArrayEntries.Value * 2;

					int previousPointer = endOfVariableLengthArray;
					foreach (short entry in VariableLengthOffsetArray)
					{
						yield return new ArraySegment<byte>(DB.Data[Page.FileID], Index + previousPointer, entry - previousPointer);
						
						previousPointer = entry;
					}
				}
			}
		}

		public RawPrimaryRecord(int index, RawPage page, RawDatabase database)
			: base (index, page, database)
		{ }
	}
}