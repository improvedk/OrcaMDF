using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlBigInt : SqlTypeBase
	{
		public SqlBigInt(CompressionContext compression)
			: base(compression)
		{ }

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get { return 8; }
		}

		public override object GetValue(byte[] value)
		{
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				// A value of 0 bytes indicates a 0-value when compressed. A 1-byte value isn't treated as a tinyint, but
				// instead as a signed one-byte value with a range of -128 through 127. For the 8-byte bigint values we rely
				// on integer overflow as we can't add a ulong to a long.
				switch (value.Length)
				{
					case 0:
						return 0;

					case 1:
						return (long)(-128 + value[0]);

					case 2:
						return (long)(-32768 + BitConverter.ToUInt16(new[] { value[1], value[0] }, 0));

					case 3:
						return (long)(-8388608 + BitConverter.ToUInt32(new byte[] { value[2], value[1], value[0], 0 }, 0));

					case 4:
						return (long)(-2147483648 + BitConverter.ToUInt32(new[] { value[3], value[2], value[1], value[0] }, 0));

					case 5:
						return (long)(-549755813888 + BitConverter.ToInt64(new byte[] { value[4], value[3], value[2], value[1], value[0], 0, 0, 0 }, 0));

					case 6:
						return (long)(-140737488355328 + BitConverter.ToInt64(new byte[] { value[5], value[4], value[3], value[2], value[1], value[0], 0, 0 }, 0));

					case 7:
						return (long)(-36028797018963968 + BitConverter.ToInt64(new byte[] { value[6], value[5], value[4], value[3], value[2], value[1], value[0], 0 }, 0));

					case 8:
						return (long)(-9223372036854775808 + BitConverter.ToInt64(new[] { value[7], value[6], value[5], value[4], value[3], value[2], value[1], value[0] }, 0));

					default:
						throw new ArgumentException("Invalid value length: " + value.Length);
				}
			}
			else
			{
				if (value.Length != 8)
					throw new ArgumentException("Invalid value length: " + value.Length);

				return BitConverter.ToInt64(value, 0);
			}
		}
	}
}