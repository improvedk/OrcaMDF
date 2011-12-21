using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlUniquifier : SqlTypeBase
	{
		public SqlUniquifier(CompressionContext compression)
			: base(compression)
		{ }

		public override bool IsVariableLength
		{
			get { return true; }
		}

		public override short? FixedLength
		{
			get { return null; }
		}

		public override object GetValue(byte[] value)
		{
			// If uniquifier has a value, convert to int
			if (value.Length == 4)
				return BitConverter.ToInt32(value, 0);
			
			// If variable length == 0, the value will implicitly be 0
			return 0;
		}
	}
}