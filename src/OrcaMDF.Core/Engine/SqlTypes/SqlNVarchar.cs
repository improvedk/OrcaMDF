using OrcaMDF.Framework.SCSU;
using System.Text;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlNVarchar : SqlTypeBase
	{
		public SqlNVarchar(CompressionContext compression)
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
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				var expander = new ScsuExpander();
				return expander.Expand(value);
			}
			else
			{
				return Encoding.Unicode.GetString(value);
			}
		}
	}
}