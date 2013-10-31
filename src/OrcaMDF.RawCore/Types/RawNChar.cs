using System.Text;

namespace OrcaMDF.RawCore.Types
{
	public class RawNChar : RawType, IRawFixedLengthType
	{
		public short Length { get; private set; }
		
		public RawNChar(string name, short length) : base(name)
		{
			Length = (short)(length * 2);
		}

		public override object GetValue(byte[] bytes)
		{
			return Encoding.Unicode.GetString(bytes);
		}
	}
}