using System;
using System.Text;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlChar : SqlTypeBase
	{
		private readonly short length;

		public SqlChar(short length, CompressionContext compression)
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

				return Encoding.UTF7.GetString(value);
			}
			else
			{
				if (value.Length != length)
					throw new ArgumentException("Invalid value length: " + value.Length);

				return Encoding.UTF7.GetString(value);
			}
		}
	}
}