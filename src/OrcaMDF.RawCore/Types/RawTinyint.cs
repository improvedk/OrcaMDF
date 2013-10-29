namespace OrcaMDF.RawCore.Types
{
	public class RawTinyint : RawType, IRawFixedLengthType
	{
		public short Length { get { return 1; } }

		public RawTinyint(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			return bytes[0];
		}
	}
}