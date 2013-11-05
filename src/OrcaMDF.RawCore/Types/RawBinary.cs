namespace OrcaMDF.RawCore.Types
{
	public class RawBinary : RawType, IRawFixedLengthType
	{
		public short Length { get; private set; }
		
		public RawBinary(string name, short length) : base(name)
		{
			Length = length;
		}

		public override object GetValue(byte[] bytes)
		{
			return bytes;
		}
	}
}