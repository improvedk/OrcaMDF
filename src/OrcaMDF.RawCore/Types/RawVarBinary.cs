namespace OrcaMDF.RawCore.Types
{
	public class RawVarBinary : RawType, IRawVariableLengthType
	{
		public object EmptyValue { get { return new byte[0]; } }

		public RawVarBinary(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes;
		}
	}
}