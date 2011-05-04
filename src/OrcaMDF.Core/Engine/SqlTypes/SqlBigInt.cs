using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlBigInt : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 8; }
		}

		public object GetValue(byte[] value)
		{
			if (value.Length != 8)
				throw new ArgumentException("Invalid value length: " + value.Length);

			return BitConverter.ToInt64(value, 0);
		}
	}
}