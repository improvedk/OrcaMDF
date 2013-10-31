namespace OrcaMDF.RawCore.Types
{
	public class RawHierarchyID : RawType, IRawVariableLengthType
	{
		public RawHierarchyID(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes;
		}
	}
}