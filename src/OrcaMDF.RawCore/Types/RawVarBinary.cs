namespace OrcaMDF.RawCore.Types
{
	public class RawVarBinary : RawType, IRawVariableLengthType
	{
		public RawVarBinary(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes;
		}
	}
}