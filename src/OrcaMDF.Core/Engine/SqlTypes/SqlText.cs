using System;
using System.Text;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlText : SqlTypeBase
	{
		public SqlText(CompressionContext compression)
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
			return Encoding.UTF7.GetString(value);
		}
	}
}