namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlVarBinary : ISqlType
	{
		public bool IsVariableLength
		{
			get { return true; }
		}

		public short? FixedLength
		{
			get { return null; }
		}

		public object GetValue(byte[] value)
		{
			return value;
		}
	}
}