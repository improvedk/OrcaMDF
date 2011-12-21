using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlRID : SqlTypeBase
	{
		public SqlRID(CompressionContext compression)
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
			if (value.Length != 8)
				throw new ArgumentException("Invalid value length: " + value.Length);

			return new SlotPointer(value);
		}
	}
}