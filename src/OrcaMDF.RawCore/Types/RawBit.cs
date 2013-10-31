using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawBit : RawType, IRawFixedLengthType
	{
		public short Length { get { return 1; } }

		public RawBit(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			throw new InvalidOperationException("You're not supposed to call GetValue on bit types.");
		}
	}
}