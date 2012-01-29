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
			// If value is compressed, a 0-byte value actually indicates a 0-value
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				if (value.Length > 1)
					throw new ArgumentException("Invalid value length: " + value.Length);

				if (value.Length == 0)
					return 0;
				else
					return value[0];
			}
			else
			{
				if (value.Length != 1)
					throw new ArgumentException("Invalid value length: " + value.Length);

				return value[0];	
			}
		}
	}
}