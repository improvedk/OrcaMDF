using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawSmallInt : RawType, IRawFixedLengthType
	{
		public short Length { get { return 2; } }

		public RawSmallInt(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return BitConverter.ToInt16(bytes, 0);
		}
	}
}