using System.Text;

namespace OrcaMDF.RawCore.Types
{
	public class RawNVarchar : RawType, IRawVariableLengthType
	{
		public object EmptyValue { get { return ""; } }

		public RawNVarchar(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return Encoding.Unicode.GetString(bytes);
		}
	}
}