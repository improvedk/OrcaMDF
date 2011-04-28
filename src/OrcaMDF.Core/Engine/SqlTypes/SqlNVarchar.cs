using System.Text;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlNVarchar : ISqlType
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