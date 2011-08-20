using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlDecimal : ISqlType
	{
		private byte precision;
		private byte scale;

		public SqlDecimal(byte precision, byte scale)
		{
			this.precision = precision;
			this.scale = scale;
		}

		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
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

		public object GetValue(byte[] value)
		{
			if (value.Length != FixedLength.Value)
				throw new ArgumentException("Invalid value length: " + value.Length);

			int[] ints = new int[4];

			for(int i=0; i<getNumberOfRequiredStorageInts(); i++)
				ints[i] = BitConverter.ToInt32(value, 1 + i * 4);

			var sqlDecimal = new System.Data.SqlTypes.SqlDecimal(precision, scale, Convert.ToBoolean(value[0]), ints);

			// This will fail for any SQL Server decimal values increasing the max value of a C# decimal.
			// Might want to return raw bytes at some point, though it's ugly.
			return sqlDecimal.Value;
		}
	}
}