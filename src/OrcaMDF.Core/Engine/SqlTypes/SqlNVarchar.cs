using System;
using System.Text;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlNVarchar : ISqlType
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
			return Encoding.Unicode.GetString(value);
		}
	}
}