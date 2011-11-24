using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlSmallMoney : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 4; }
		}

		public object GetValue(byte[] value)
		{
			if (value.Length != FixedLength.Value)
				throw new ArgumentException("Invalid value length: " + value.Length);

			// Fixed decimal point for smallmoney
			return BitConverter.ToInt32(value, 0) / 10000m;
		}
	}
}