using System;
using OrcaMDF.Core.Framework;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlSmallInt : SqlTypeBase
	{
		public SqlSmallInt(CompressionContext compression)
			: base(compression)
		{ }

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get { return 2; }
		}

		public override object GetValue(byte[] value)
		{
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				if (value.Length > 2)
					throw new ArgumentException("Invalid value length: " + value.Length);

				return SqlBitConverter.ToInt16FromBigEndian(value, 0, Offset.MinValue);
			}
			else
			{
				if (value.Length != 2)
					throw new ArgumentException("Invalid value length: " + value.Length);
				
				return BitConverter.ToInt16(value, 0);
			}
		}
	}
}