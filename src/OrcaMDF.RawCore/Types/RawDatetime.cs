using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawDatetime : IRawFixedLengthType
	{
		private const double CLOCK_TICK_MS = 10d / 3d;

		public short Length { get { return 8; } }

		public object GetValue(byte[] bytes)
		{
			int time = BitConverter.ToInt32(bytes, 0);
			int date = BitConverter.ToInt32(bytes, 4);

			return new DateTime(1900, 1, 1).AddMilliseconds(time * CLOCK_TICK_MS).AddDays(date);
		}
	}
}