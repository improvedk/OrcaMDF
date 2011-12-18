using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlUniquifier : ISqlType
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
			// If uniquifier has a value, convert to int
			if (value.Length == 4)
				return BitConverter.ToInt32(value, 0);
			
			// If variable length == 0, the value will implicitly be 0
			return 0;
		}
	}
}