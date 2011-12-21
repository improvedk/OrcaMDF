using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlTinyInt : SqlTypeBase
	{
		public SqlTinyInt(CompressionContext compression)
			: base(compression)
		{ }

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get { return 1; }
		}

		public override object GetValue(byte[] value)
		{
			if (value.Length != 1)
				throw new ArgumentException("Invalid value length: " + value.Length);

			return value[0];
		}
	}
}