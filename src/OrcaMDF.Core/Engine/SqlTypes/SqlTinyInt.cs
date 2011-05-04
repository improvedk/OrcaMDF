using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlTinyInt : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 1; }
		}

		public object GetValue(byte[] value)
		{
			if (value.Length != 1)
				throw new ArgumentException("Invalid value length: " + value.Length);

			return value[0];
		}
	}
}