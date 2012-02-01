using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlDateTime : SqlTypeBase
	{
		private const double CLOCK_TICK_MS = 10d/3d;

		public SqlDateTime(CompressionContext compression)
			: base(compression)
		{ }

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get { return 8; }
		}

		public override object GetValue(byte[] value)
		{
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				var date = new DateTime(1900, 1, 1);

				if (value.Length > 8)
					throw new ArgumentException("Invalid value length: " + value.Length);

				// If date is above four, we know it contains a 4-byte big-endian time part + a variable width date part
				if (value.Length > 4)
				{
					// Add time
					var timeBytes = new[]
						{
							value[value.Length - 1],
							value[value.Length - 2],
							value[value.Length - 3],
							value[value.Length - 4]
						};
					date = date.AddMilliseconds(BitConverter.ToUInt32(timeBytes, 0) * CLOCK_TICK_MS);

					// Add days
					switch (value.Length)
					{
						case 5:
							date = date.AddDays(-128 + value[0]);
							break;

						case 6:
							date = date.AddDays(-32768 + BitConverter.ToUInt16(new[] { value[1], value[0] }, 0));
							break;

						case 7:
							date = date.AddDays(-8388608 + BitConverter.ToUInt32(new byte[] { value[2], value[1], value[0], 0 }, 0));
							break;

						case 8:
							date = date.AddDays(-2147483648 + BitConverter.ToUInt32(new[] { value[3], value[2], value[1], value[0] }, 0));
							break;
					}
				}
				else if (value.Length > 0)
				{
					// Otherwise the date only consists of a variable-width big-endian time part
					switch (value.Length)
					{
						case 1:
							date = date.AddMilliseconds(value[0] * CLOCK_TICK_MS);
							break;

						case 2:
							date = date.AddMilliseconds((-32768 + BitConverter.ToUInt16(new[] { value[1], value[0] }, 0)) * CLOCK_TICK_MS);
							break;

						case 3:
							date = date.AddMilliseconds((-8388608 + BitConverter.ToUInt32(new byte[] { value[2], value[1], value[0], 0 }, 0)) * CLOCK_TICK_MS);
							break;

						case 4:
							date = date.AddMilliseconds((-2147483648 + BitConverter.ToUInt32(new[] { value[3], value[2], value[1], value[0] }, 0)) * CLOCK_TICK_MS);
							break;
					}
				}

				return date;
			}
			else
			{
				if (value.Length != 8)
					throw new ArgumentException("Invalid value length: " + value.Length);

				int time = BitConverter.ToInt32(value, 0);
				int date = BitConverter.ToInt32(value, 4);

				return new DateTime(1900, 1, 1).AddMilliseconds(time * CLOCK_TICK_MS).AddDays(date);
			}
		}
	}
}