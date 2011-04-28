using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlSmallInt : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 2; }
		}

		public object GetValue(byte[] value)
		{
			return BitConverter.ToInt16(value, 0);
		}
	}
}