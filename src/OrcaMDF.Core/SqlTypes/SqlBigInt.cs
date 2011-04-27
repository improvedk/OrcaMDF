using System;

namespace OrcaMDF.Core.SqlTypes
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
			return BitConverter.ToInt64(value, 0);
		}
	}
}