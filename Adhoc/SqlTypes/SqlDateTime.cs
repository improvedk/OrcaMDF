using System;

namespace Orca.MdfReader.Adhoc.SqlTypes
{
	public class SqlDateTime : ISqlType
	{
		private const double CLOCK_TICK_MS = 10d/3d;

		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 8; }
		}

		public object GetValue(byte[] value)
		{
			int time = BitConverter.ToInt32(value, 0);
			int date = BitConverter.ToInt32(value, 4);

			return new DateTime(1900, 1, 1, time/300/60/60, time/300/60%60, time/300%60, (int)(time%300*CLOCK_TICK_MS)).AddDays(date);
		}
	}
}