using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlIntTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlInt();
			byte[] input;

			input = new byte[] { 0x5e, 0x3b, 0x27, 0x2a };
			Assert.AreEqual(707214174, Convert.ToInt32(type.GetValue(input)));

			input = new byte[] { 0x8d, 0xf9, 0xaa, 0x30 };
			Assert.AreEqual(816511373, Convert.ToInt32(type.GetValue(input)));
			
			input = new byte[] { 0x7a, 0x4a, 0x72, 0xe2 };
			Assert.AreEqual(-495826310, Convert.ToInt32(type.GetValue(input)));
		}

		[Test]
		public void Length()
		{
			var type = new SqlInt();

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[3]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[5]));
		}
	}
}