using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawUniqueIdentifier : RawType, IRawFixedLengthType
	{
		public short Length { get { return 16; } }

		public RawUniqueIdentifier(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return new Guid(bytes);
		}
	}
}