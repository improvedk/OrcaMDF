using System;
using System.Collections;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlDecimal : SqlTypeBase
	{
		private readonly byte precision;
		private readonly byte scale;

		public SqlDecimal(byte precision, byte scale, CompressionContext compression)
			: base(compression)
		{
			this.precision = precision;
			this.scale = scale;
		}

		public override bool IsVariableLength
		{
			get { return CompressionContext.UsesVardecimals; }
		}

		public override short? FixedLength
		{
			get { return Convert.ToInt16(1 + getNumberOfRequiredStorageInts() * 4); }
		}

		private byte getNumberOfRequiredStorageInts()
		{
			if (precision <= 9)
				return 1;

			if (precision <= 19)
				return 2;

			if (precision <= 28)
				return 3;

			return 4;
		}

		public override object GetValue(byte[] value)
		{
			if(!CompressionContext.UsesVardecimals)
			{
				if (value.Length != FixedLength.Value)
					throw new ArgumentException("Invalid value length: " + value.Length);

				int[] ints = new int[4];

				for (int i = 0; i < getNumberOfRequiredStorageInts(); i++)
					ints[i] = BitConverter.ToInt32(value, 1 + i * 4);

				var sqlDecimal = new System.Data.SqlTypes.SqlDecimal(precision, scale, Convert.ToBoolean(value[0]), ints);

				// This will fail for any SQL Server decimal values exceeding the max value of a C# decimal.
				// Might want to return raw bytes at some point, though it's ugly.
				return sqlDecimal.Value;
			}
			else
			{
				// Zero values are simply stored as a 0-length variable length field
				if (value.Length == 0)
					return 0m;

				// Sign is stored in the first bit of the first byte
				decimal sign = (value[0] >> 7) == 1 ? 1 : -1;

				// Exponent is stored in the remaining 7 bytes of the first byte. As it's biased by 64 (ensuring we won't
				// have to deal with negative numbers) we need to subtract the bias to get the real exponent value.
				byte exponent = (byte)((value[0] & 127) - 64);
				
				// Mantissa is stored in the remaining bytes, in chunks of 10 bits
				int totalBits = (value.Length - 1) * 8;
				int mantissaChunks = (int)Math.Ceiling(totalBits / 10d);
				var mantissaBits = new BitArray(value);
				
				// Loop each chunk, adding the value to the total mantissa value
				decimal mantissa = 0;
				int bitPointer = 8;

				for (int chunk = mantissaChunks; chunk > 0; chunk--)
				{
					// The cumulative value for this 10-bit chunk
					decimal chunkValue = 0;

					// For each bit in the chunk, shift it into position, provided it's set
					for (int chunkBit = 9; chunkBit >= 0; chunkBit--)
					{
						// Since we're looping bytes left-to-right, but read bits right-to-left, we need
						// to transform the bit pointer into a relative index within the current byte.
						int byteAwareBitPointer = bitPointer + 7 - bitPointer % 8 - (7 - (7 - bitPointer % 8));

						// If the bit is set and it's available (SQL Server will truncate 0's), shift it into position
						if (mantissaBits.Length > bitPointer && mantissaBits[byteAwareBitPointer])
							chunkValue += (1 << chunkBit);

						bitPointer++;
					}

					// Once all bits are in position, we need to raise the significance according to the chunk
					// position. First chunk is most significant, last is the least significant. Each chunk
					// defining three digits.
					mantissa += chunkValue * (decimal)Math.Pow(10, (chunk - 1) * 3);
				}

				// Mantissa has hardcoded decimal place after first digit
				mantissa = mantissa / (decimal)Math.Pow(10, Math.Floor(Math.Log10((double)mantissa)));

				// Apply sign and multiply by the exponent
				return sign * mantissa * (decimal)Math.Pow(10, exponent);
			}
		}
	}
}