using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawInt : IRawFixedLengthType
	{
		public short Length { get { return 4; } }

		public object GetValue(byte[] bytes)
		{
			return BitConverter.ToInt32(bytes, 0);
		}
	}
}