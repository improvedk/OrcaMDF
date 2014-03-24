namespace OrcaMDF.RawCore.Types
{
	public class RawXml : RawType, IRawVariableLengthType
	{
		public object EmptyValue { get { return ""; } }

		public RawXml(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes;
		}
	}
}