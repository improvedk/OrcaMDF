using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlSmallDateTime : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 4; }
		}

		public byte[] NormalizeCompressedValue(byte[] value)
		{
			throw new NotImplementedException();
		}

		public object GetValue(byte[] value)
		{
			if (value.Length != 4)
				throw new ArgumentException("Invalid value length: " + value.Length);

			ushort time = BitConverter.ToUInt16(value, 0);
			ushort date = BitConverter.ToUInt16(value, 2);

			return new DateTime(1900, 1, 1, time / 60, time % 60, 0).AddDays(date);
		}
	}
}