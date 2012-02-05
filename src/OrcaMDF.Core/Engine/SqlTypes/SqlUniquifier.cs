using System;
using OrcaMDF.Core.Framework;

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
			if (value.Length > 4)
				throw new ArgumentException("Invalid value length: " + value.Length);

			if (CompressionContext.CompressionLevel != CompressionLevel.None)
				return SqlBitConverter.ToInt32FromBigEndian(value, 0, Offset.MinValue);
			else
			{
				// If uniquifier has a value, convert to int
				if (value.Length == 4)
					return BitConverter.ToInt32(value, 0);

				// If variable length == 0, the value will implicitly be 0
				return 0;
			}
		}
	}
}