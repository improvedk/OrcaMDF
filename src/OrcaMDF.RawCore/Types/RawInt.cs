using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawInt : RawType, IRawFixedLengthType
	{
		public short Length { get { return 4; } }

		public RawInt(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return BitConverter.ToInt32(bytes, 0);
		}
	}
}