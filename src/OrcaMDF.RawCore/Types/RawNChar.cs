using System.Text;

namespace OrcaMDF.RawCore.Types
{
	public class RawNChar : IRawFixedLengthType
	{
		public RawNChar(short length)
		{
			Length = length;
		}

		public short Length { get; private set; }

		public object GetValue(byte[] bytes)
		{
			return Encoding.Unicode.GetString(bytes);
		}
	}
}