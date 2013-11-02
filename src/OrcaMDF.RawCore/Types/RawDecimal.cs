using System;
using System.Data.SqlTypes;

namespace OrcaMDF.RawCore.Types
{
	public class RawDecimal : RawType, IRawFixedLengthType
	{
		private readonly byte precision;
		private readonly byte scale;

		public short Length
		{
			get { return Convert.ToInt16(1 + getNumberOfRequiredStorageInts() * 4); }
		}

		public RawDecimal(string name, byte precision, byte scale) : base(name)
		{
			this.precision = precision;
			this.scale = scale;
		}

		private byte getNumberOfRequiredStorageInts()
		{
			if (precision <= 9)
				return 1;

			if (precision <= 19)
				return 2;

			if (precision <= 28)
				return 3;

			return 4;
		}

		public override object GetValue(byte[] bytes)
		{
			var ints = new int[4];

			for (int i = 0; i < getNumberOfRequiredStorageInts(); i++)
				ints[i] = BitConverter.ToInt32(bytes, 1 + i * 4);

			return new SqlDecimal(precision, scale, Convert.ToBoolean(bytes[0]), ints).Value;
		}
	}
}