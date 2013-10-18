using System;
using System.Collections.Generic;
using System.Linq;

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

		public IEnumerable<byte> FixedLengthData
		{
			get { return DB.Data[Page.FileID].Skip(Index + 4).Take(NullBitmapPointer - 4); }
		}

		public IEnumerable<byte> NullBitmapRawBytes
		{
			get { return DB.Data[Page.FileID].Skip(Index + NullBitmapPointer + 2).Take((NullBitmapColumnCount + 7) / 8); }
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

		public IEnumerable<IEnumerable<byte>> VariableLengthOffsetValues
		{
			get
			{
				if (HasVariableLengthColumns)
				{
					int endOfVariableLengthArray = pointerToEndOfNullBitmap + 2 + NumberOfVariableLengthOffsetArrayEntries.Value * 2;

					int previousPointer = endOfVariableLengthArray;
					foreach (short entry in VariableLengthOffsetArray)
					{
						yield return DB.Data[Page.FileID].Skip(Index + previousPointer).Take(entry - previousPointer);
						
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