namespace Orca.MdfReader.Adhoc.SqlTypes
{
	public class SqlBitReadState
	{
		private int currentBitIndex;
		private readonly byte bits;
		
		public SqlBitReadState(byte bits)
		{
			this.bits = bits;
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