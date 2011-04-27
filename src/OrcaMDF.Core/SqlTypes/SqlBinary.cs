namespace OrcaMDF.Core.SqlTypes
{
	public class SqlBinary : ISqlType
	{
		private short length;

		public SqlBinary(short length)
		{
			this.length = length;
		}
		
		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get { return length; }
		}

		public object GetValue(byte[] value)
		{
			return value;
		}
	}
}