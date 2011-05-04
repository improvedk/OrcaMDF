using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	[TestFixture]
	public class SqlTinyIntTests
	{
		[Test]
		public void GetValue()
		{
			var type = new SqlTinyInt();
			byte[] input;

			input = new byte[] { 0xd9 };
			Assert.AreEqual(0xd9, Convert.ToByte(type.GetValue(input)));

			input = new byte[] { 0xf9 };
			Assert.AreEqual(0xf9, Convert.ToByte(type.GetValue(input)));

			input = new byte[] { 0xa4 };
			Assert.AreEqual(0xa4, Convert.ToByte(type.GetValue(input)));
		}

		[Test]
		public void Length()
		{
			var type = new SqlTinyInt();

			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[0]));
			Assert.Throws<ArgumentException>(() => type.GetValue(new byte[2]));
		}
	}
}