using System.Text;

namespace OrcaMDF.RawCore.Types
{
	public class RawVarchar : RawType, IRawVariableLengthType
	{
		public RawVarchar(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return Encoding.ASCII.GetString(bytes);
		}
	}
}