using System;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlBinary : SqlTypeBase
	{
		private readonly short length;

		public SqlBinary(short length, CompressionContext compression)
			: base(compression)
		{
			this.length = length;
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
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				if (value.Length > length)
					throw new ArgumentException("Invalid value length: " + value.Length);

				if (value.Length < length)
				{
					var result = new byte[length];
					Array.Copy(value, result, value.Length);

					return result;
				}
				else
					return value;
			}
			else
			{
				if (value.Length != length)
					throw new ArgumentException("Invalid value length: " + value.Length);

				return value;
			}
		}
	}
}