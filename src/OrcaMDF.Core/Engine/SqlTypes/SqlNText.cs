using System.Text;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlNText : SqlTypeBase
	{
		public SqlNText(CompressionContext compression)
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
			return Encoding.Unicode.GetString(value);
		}
	}
}