using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlMoney : SqlTypeBase
	{
		public SqlMoney(CompressionContext compression)
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
			if (value.Length != FixedLength.Value)
				throw new ArgumentException("Invalid value length: " + value.Length);

			// Fixed decimal point for money
			return BitConverter.ToInt64(value, 0) / 10000m;
		}
	}
}