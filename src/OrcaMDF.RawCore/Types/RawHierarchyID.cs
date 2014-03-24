namespace OrcaMDF.RawCore.Types
{
	public class RawHierarchyID : RawType, IRawVariableLengthType
	{
		public object EmptyValue { get { return new byte[0]; } }

		public RawHierarchyID(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes;
		}
	}
}