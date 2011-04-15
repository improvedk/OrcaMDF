using System.Text;

namespace Orca.MdfReader.Adhoc.SqlTypes
{
	public class SqlChar : ISqlType
	{
		private short length;

		public SqlChar(short length)
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
			return Encoding.UTF7.GetString(value);
		}
	}
}