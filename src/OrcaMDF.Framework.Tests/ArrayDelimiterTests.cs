using NUnit.Framework;
using System;
using System.Linq;

namespace OrcaMDF.Framework.Tests
{
	[TestFixture]
	public class ArrayDelimiterTests
	{
		[Test]
		public void OutOfRangeCount()
		{
			Assert.Throws<IndexOutOfRangeException>(() => new ArrayDelimiter<byte>(new byte[0], 0, 5));
			Assert.Throws<IndexOutOfRangeException>(() => new ArrayDelimiter<byte>(new byte[10], 0, 11));
			Assert.Throws<IndexOutOfRangeException>(() => new ArrayDelimiter<byte>(new byte[10], 5, 6));
		}

		[Test]
		public void InRange()
		{
			var source = new byte[] { 1, 2, 3, 4, 5 };

			Assert.AreEqual(source, new ArrayDelimiter<byte>(source, 0, 5).ToArray());
			Assert.AreEqual(new byte[] { 2, 3, 4 }, new ArrayDelimiter<byte>(source, 1, 3).ToArray());
			Assert.AreEqual(new byte[] { 4, 5 }, new ArrayDelimiter<byte>(source, 3, 2).ToArray());
			Assert.AreEqual(new byte[] { 5 }, new ArrayDelimiter<byte>(source, 4, 1).ToArray());
			Assert.AreEqual(new byte[] { }, new ArrayDelimiter<byte>(source, 5, 0).ToArray());
			Assert.AreEqual(new byte[] { }, new ArrayDelimiter<byte>(source, 2, 0).ToArray());
		}

		[Test]
		public void OutOfRangeIndex()
		{
			Assert.Throws<IndexOutOfRangeException>(() => new ArrayDelimiter<byte>(new byte[5], 2, 3)[5].ToString());
			Assert.Throws<IndexOutOfRangeException>(() => new ArrayDelimiter<byte>(new byte[5], 2, 3)[3].ToString());
			Assert.Throws<IndexOutOfRangeException>(() => new ArrayDelimiter<byte>(new byte[5], 0, 5)[-1].ToString());
			Assert.Throws<IndexOutOfRangeException>(() => new ArrayDelimiter<byte>(new byte[5], 0, 5)[6].ToString());
		}
	}
}