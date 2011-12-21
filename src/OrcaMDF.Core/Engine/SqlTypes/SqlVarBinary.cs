namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlVarBinary : SqlTypeBase
	{
		public SqlVarBinary(CompressionContext compression)
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
			return value;
		}
	}
}