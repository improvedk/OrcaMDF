using NUnit.Framework;
using OrcaMDF.Core.Engine.Records;

namespace OrcaMDF.Core.Tests.Engine.Records
{
	[TestFixture]
	public class RecordReadStateTests
	{
		[Test]
		public void General()
		{
			var state = new RecordReadState();

			// No bits available
			Assert.IsTrue(state.AllBitsConsumed);

			state.LoadBitByte(0xD2); // 11010010

			// Bits available
			Assert.IsFalse(state.AllBitsConsumed);

			// Reading bit values
			Assert.IsFalse(state.GetNextBit());
			Assert.IsTrue(state.GetNextBit());
			Assert.IsFalse(state.GetNextBit());
			Assert.IsFalse(state.GetNextBit());
			Assert.IsTrue(state.GetNextBit());
			Assert.IsFalse(state.GetNextBit());
			Assert.IsTrue(state.GetNextBit());

			// One bit left
			Assert.IsFalse(state.AllBitsConsumed);

			Assert.IsTrue(state.GetNextBit());

			// Bits exhausted, ready for next byte
			Assert.IsTrue(state.AllBitsConsumed);
		}
	}
}