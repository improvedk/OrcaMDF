using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlSmallInt : SqlTypeBase
	{
		public SqlSmallInt(CompressionContext compression)
			: base(compression)
		{ }

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get { return 2; }
		}

		public override object GetValue(byte[] value)
		{
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				// A value of 0 bytes indicates a 0-value when compressed. A 1-byte value isn't treated as a tinyint, but
				// instead as a signed one-byte value with a range of -128 through 127.
				switch (value.Length)
				{
					case 0:
						return 0;

					case 1:
						return (short)(-128 + value[0]);

					case 2:
						return (short)(-32768 + BitConverter.ToUInt16(new[] { value[1], value[0] }, 0));

					default:
						throw new ArgumentException("Invalid value length: " + value.Length);
				}
			}
			else
			{
				if (value.Length != 2)
					throw new ArgumentException("Invalid value length: " + value.Length);
				
				return BitConverter.ToInt16(value, 0);
			}
		}
	}
}