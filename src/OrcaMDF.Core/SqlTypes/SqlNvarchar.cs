using System.Text;

namespace OrcaMDF.Core.SqlTypes
{
	public class SqlNvarchar : ISqlType
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
			return Encoding.Unicode.GetString(value);
		}
	}
}