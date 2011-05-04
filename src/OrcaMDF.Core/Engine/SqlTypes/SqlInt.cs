using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlInt : ISqlType
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
			if (value.Length != 4)
				throw new ArgumentException("Invalid value length: " + value.Length);

			return BitConverter.ToInt32(value, 0);
		}
	}
}