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
				// TODO: Implement SCSU unicode decompression
				return Encoding.UTF8.GetString(value);
			}
			else
			{
				return Encoding.Unicode.GetString(value);
			}
		}
	}
}