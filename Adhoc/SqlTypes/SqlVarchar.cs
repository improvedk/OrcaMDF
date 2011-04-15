using System.Text;

namespace Orca.MdfReader.Adhoc.SqlTypes
{
	public class SqlVarchar : ISqlType
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
			return Encoding.UTF7.GetString(value);
		}
	}
}