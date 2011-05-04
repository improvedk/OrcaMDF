using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlCharTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlChar(5);
			byte[] input;

			input = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
			Assert.AreEqual("Hello", type.GetValue(input));
		}

		[Test]
		public void Length()
		{
			var type = new SqlChar(5);

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[4]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[6]));
		}
	}
}