using System.Text;

namespace OrcaMDF.Core.SqlTypes
{
	public class SqlNChar : ISqlType
	{
		private short length;

		public SqlNChar(short length)
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
			return Encoding.Unicode.GetString(value);
		}
	}
}