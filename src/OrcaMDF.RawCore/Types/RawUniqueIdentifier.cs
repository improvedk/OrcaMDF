using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawUniqueIdentifier : IRawFixedLengthType
	{
		public short Length { get { return 16; } }

		public object GetValue(byte[] bytes)
		{
			return new Guid(bytes);
		}
	}
}