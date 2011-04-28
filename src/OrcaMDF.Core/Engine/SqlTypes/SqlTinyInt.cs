namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlTinyInt : ISqlType
	{
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return 1; }
		}

		public object GetValue(byte[] value)
		{
			return value[0];
		}
	}
}