namespace OrcaMDF.Core.Engine.Records
{
	public class RecordReadState
	{
		// We start out having consumed all bits as none have been read
		private int currentBitIndex = 8;
		private byte bits;

		public void LoadBitByte(byte bits)
		{
			this.bits = bits;
			currentBitIndex = 0;
		}

		public bool AllBitsConsumed
		{
			get { return currentBitIndex == 8; }
		}

		public bool GetNextBit()
		{
			return (bits & (1 << currentBitIndex++)) != 0;
		}
	}
}