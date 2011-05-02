using System;
using System.Linq;
using System.Text;

namespace OrcaMDF.Core.Engine.SqlTypes
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
			if (value.Length < length)
				throw new ArgumentException("Data too shart for char(" + length + ")");

			if (value.Length > length)
				return Encoding.UTF7.GetString(value.Take(length).ToArray());
			else
				return Encoding.UTF7.GetString(value);
		}
	}
}