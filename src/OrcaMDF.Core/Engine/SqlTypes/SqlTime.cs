using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlTime : SqlTypeBase
	{
		private readonly byte scale;
		private readonly byte length;

		public SqlTime(byte scale, CompressionContext compression)
			: base(compression)
		{
			this.scale = scale;

			if (scale <= 2)
				length = 3;
			else if (scale <= 4)
				length = 4;
			else if (scale <= 7)
				length = 5;
			else
				throw new ArgumentException("Invalid scale: " + scale);
		}

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get { return length; }
		}

		public override object GetValue(byte[] value)
		{
			if (value.Length != length)
				throw new ArgumentException("Invalid value length: " + value.Length);

			// Magic needed to read a 3 byte integer into .NET's 4 byte representation.
			// Reading backwards due to assumed little endianness.
			long timeVal = (value[2] << 16) + (value[1] << 8) + value[0];
			if (length >= 4)
				timeVal += (long)(value[3]) << 24;
			if (length >= 5)
				timeVal += (long)(value[4]) << 32;

			int time = (int)(timeVal / Math.Pow(10, scale));
			int fraction = (int)(timeVal % Math.Pow(10, scale));

			return new TimeSpan(0, time / 60 / 60, time / 60 % 60, time % 60, fraction);
		}
	}
}