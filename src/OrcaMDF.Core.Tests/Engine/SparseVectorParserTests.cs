using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Engine
{
	[TestFixture]
	public class SparseVectorParserTests
	{
		[Test]
		public void Parse()
		{
			byte[] bytes = new byte [] { 0x05, 0x00, 0x02, 0x00, 0x03, 0x00, 0x06, 0x00, 0x10, 0x00, 0x14, 0x00, 0x03, 0x00, 0x00, 0x00, 0xd2, 0x04, 0x00, 0x00 };
			var parser = new SparseVectorParser(bytes);

			Assert.AreEqual(2, parser.ColumnCount);
			Assert.AreEqual(3, BitConverter.ToInt32(parser.ColumnValues[3], 0));
			Assert.AreEqual(1234, BitConverter.ToInt32(parser.ColumnValues[6], 0));
		}
	}
}