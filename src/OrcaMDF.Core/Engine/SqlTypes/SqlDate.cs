using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlDate : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 3; }
		}

		public object GetValue(byte[] value)
		{
			if (value.Length != 3)
				throw new ArgumentException("Invalid value length: " + value.Length);

			// Magic needed to read a 3 byte integer into .NET's 4 byte representation.
			// Reading backwards due to assumed little endianness.
			int date = (value[2] << 16) + (value[1] << 8) + value[0];

			return new DateTime(1, 1, 1).AddDays(date);
		}
	}
}