using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawBigInt : RawType, IRawFixedLengthType
	{
		public short Length { get { return 8; } }

		public RawBigInt(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return BitConverter.ToInt64(bytes, 0);
		}
	}
}