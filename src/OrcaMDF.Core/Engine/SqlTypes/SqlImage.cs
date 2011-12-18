using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlImage : ISqlType
	{
		public bool IsVariableLength
		{
			get { return true; }
		}

		public short? FixedLength
		{
			get { return null; }
		}

		public byte[] NormalizeCompressedValue(byte[] value)
		{
			throw new NotImplementedException();
		}

		public object GetValue(byte[] value)
		{
			return value;
		}
	}
}