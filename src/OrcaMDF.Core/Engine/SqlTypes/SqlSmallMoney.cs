using OrcaMDF.Framework;
using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlSmallMoney : SqlTypeBase
	{
		public SqlSmallMoney(CompressionContext compression)
			: base(compression)
		{ }

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get { return 4; }
		}

		public override object GetValue(byte[] value)
		{
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				if (value.Length > 4)
					throw new ArgumentException("Invalid value length: " + value.Length);

				return SqlBitConverter.ToInt32FromBigEndian(value, 0, Offset.MinValue) / 10000m;
			}
			else
			{
				if (value.Length != FixedLength.Value)
					throw new ArgumentException("Invalid value length: " + value.Length);

				// Fixed decimal point for smallmoney
				return BitConverter.ToInt32(value, 0) / 10000m;
			}
		}
	}
}