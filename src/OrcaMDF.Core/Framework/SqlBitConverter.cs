using System;

namespace OrcaMDF.Core.Framework
{
	/// <summary>
	/// Works very similar to the native BitConverter, except:
	///  - Supports big endian input
	///  - Supports integers based on the minimum value instead of 0
	///  - Will automatically pad insignificant zeroes if there are not enough input bytes
	/// </summary>
	public static class SqlBitConverter
	{
		public static short ToInt16FromBigEndian(byte[] input, int index, Offset offset)
		{
			if (input.Length == 0)
				return 0;

		 	if (index >= input.Length)
		 		throw new ArgumentOutOfRangeException("index");

			// If we're offset from the MinValue, calculate the lowest value possible with the given amount of input bytes
			short offsetValue = (short)(offset == Offset.Zero ? 0 : (-1 * (1 << Math.Min(input.Length - index, sizeof(short)) * 8 - 1)));

			switch (input.Length - index)
			{
				case 1:
					return (short)(offsetValue + input[index]);

				default:
					return (short)(offsetValue + (input[index] << 8 | input[index + 1]));
			}
		}
		
		public static int ToInt32FromBigEndian(byte[] input, int index, Offset offset)
		{
			if (input.Length == 0)
				return 0;

			if (index >= input.Length)
				throw new ArgumentOutOfRangeException("index");

			// If we're offset from the MinValue, calculate the lowest value possible with the given amount of input bytes
			int offsetValue = offset == Offset.Zero ? 0 : (-1 * (1 << Math.Min(input.Length - index, sizeof(int)) * 8 - 1));

			switch (input.Length - index)
			{
				case 1:
					return offsetValue + input[index];

				case 2:
					return offsetValue + (input[index] << 8 | input[index + 1]);

				case 3:
					return offsetValue + (input[index] << 16 | input[index + 1] << 8 | input[index + 2]);

				default:
					return offsetValue + (input[index] << 24 | input[index + 1] << 16 | input[index + 2] << 8 | input[index + 3]);
			}
		}
		
		public static long ToInt64FromBigEndian(byte[] input, int index, Offset offset)
		{
			if (input.Length == 0)
				return 0;

			if (index >= input.Length)
				throw new ArgumentOutOfRangeException("index");

			// If we're offset from the MinValue, calculate the lowest value possible with the given amount of input bytes
			long offsetValue = offset == Offset.Zero ? 0 : (-1 * ((long)1 << Math.Min(input.Length - index, sizeof(long)) * 8 - 1));

			switch (input.Length - index)
			{
				case 1:
					return offsetValue + input[index];

				case 2:
					return offsetValue + (input[index] << 8 | input[index + 1]);

				case 3:
					return offsetValue + (input[index] << 16 | input[index + 1] << 8 | input[index + 2]);

				case 4:
					return (int)offsetValue + (input[index] << 24 | input[index + 1] << 16 | input[index + 2] << 8 | input[index + 3]);

				case 5:
					return offsetValue + ((long)input[index] << 32 | (long)input[index + 1] << 24 | input[index + 2] << 16 | input[index + 3] << 8 | input[index + 4]);

				case 6:
					return offsetValue + ((long)input[index] << 40 | (long)input[index + 1] << 32 | (long)input[index + 2] << 24 | input[index + 3] << 16 | input[index + 4] << 8 | input[index + 5]);

				case 7:
					return offsetValue + ((long)input[index] << 48 | (long)input[index + 1] << 40 | (long)input[index + 2] << 32 | (long)input[index + 3] << 24 | input[index + 4] << 16 | input[index + 5] << 8 | input[index + 6]);
					
				default:
					return offsetValue + ((long)input[index] << 56 | (long)input[index + 1] << 48 | (long)input[index + 2] << 40 | (long)input[index + 3] << 32 | (long)input[index + 4] << 24 | input[index + 5] << 16 | input[index + 6] << 8 | input[index + 7]);
			}
		}
	}
}