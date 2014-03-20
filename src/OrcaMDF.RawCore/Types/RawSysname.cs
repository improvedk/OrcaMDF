using System.Text;

namespace OrcaMDF.RawCore.Types
{
	public class RawSysname : RawNVarchar
	{
		public RawSysname(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return Encoding.Unicode.GetString(bytes);
		}
	}
}