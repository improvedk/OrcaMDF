namespace OrcaMDF.RawCore.Types
{
	public class RawXml : RawType, IRawVariableLengthType
	{
		public RawXml(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes;
		}
	}
}