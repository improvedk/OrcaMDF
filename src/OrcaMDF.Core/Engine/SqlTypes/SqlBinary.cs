using System;
using System.Linq;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlBinary : ISqlType
	{
		private short length;

		public SqlBinary(short length)
		{
			this.length = length;
		}
		
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return length; }
		}

		public object GetValue(byte[] value)
		{
			if (value.Length != length)
				throw new ArgumentException("Invalid value length: " + value.Length);

			return value;
		}
	}
}