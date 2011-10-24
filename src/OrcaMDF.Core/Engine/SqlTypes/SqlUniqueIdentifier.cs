using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlUniqueIdentifier : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 16; }
		}

		public object GetValue(byte[] value)
		{
			// Uniqueidentifier is always 16 bytes
			if (value.Length != 16)
				throw new ArgumentException("Invalid value length: " + value.Length);

			return new Guid(value);
		}
	}
}