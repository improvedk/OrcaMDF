namespace OrcaMDF.RawCore.Types
{
	public class RawTinyInt : RawType, IRawFixedLengthType
	{
		public short Length { get { return 1; } }

		public RawTinyInt(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes[0];
		}
	}
}