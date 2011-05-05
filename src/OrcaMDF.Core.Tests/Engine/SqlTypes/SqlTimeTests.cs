using System;
using NUnit.Framework;
using OrcaMDF.Core.Engine.SqlTypes;

namespace OrcaMDF.Core.Tests.Engine.SqlTypes
{
	public class SqlTimeTests
	{
		[Test]
		public void GetValue()
		{
			// time(0)
			var time0 = new SqlTime(0);
			var input0 = new byte[] { 0xf9, 0x9f, 0x00 };
			Assert.AreEqual(new TimeSpan(11, 22, 33), (TimeSpan)time0.GetValue(input0));

			// time(1)
			var time1 = new SqlTime(1);
			var input1 = new byte[] { 0xbb, 0x3f, 0x06 };
			Assert.AreEqual(new TimeSpan(0, 11, 22, 33, 1), (TimeSpan)time1.GetValue(input1));

			// time(2)
			var time2 = new SqlTime(2);
			var input2 = new byte[] { 0x50, 0x7d, 0x3e };
			Assert.AreEqual(new TimeSpan(0, 11, 22, 33, 12), (TimeSpan)time2.GetValue(input2));

			// time(3)
			var time3 = new SqlTime(3);
			var input3 = new byte[] { 0x23, 0xe5, 0x70, 0x02 };
			Assert.AreEqual(new TimeSpan(0, 11, 22, 33, 123), (TimeSpan)time3.GetValue(input3));

			// time(4)
			var time4 = new SqlTime(4);
			var input4 = new byte[] { 0x63, 0xf3, 0x68, 0x18 };
			Assert.AreEqual(new TimeSpan(0, 11, 22, 33, 1235), (TimeSpan)time4.GetValue(input4));

			// time(5)
			var time5 = new SqlTime(5);
			var input5 = new byte[] { 0xda, 0x81, 0x19, 0xf4, 0x00 };
			Assert.AreEqual(new TimeSpan(0, 11, 22, 33, 12346), (TimeSpan)time5.GetValue(input5));

			// time(6)
			var time6 = new SqlTime(6);
			var input6 = new byte[] { 0x81, 0x12, 0xff, 0x88, 0x09 };
			Assert.AreEqual(new TimeSpan(0, 11, 22, 33, 123457), (TimeSpan)time6.GetValue(input6));

			// time(7)
			var time7 = new SqlTime(7);
			var input7 = new byte[] { 0x07, 0xb9, 0xf6, 0x59, 0x5f };
			Assert.AreEqual(new TimeSpan(0, 11, 22, 33, 1234567), (TimeSpan)time7.GetValue(input7));
		}

		[Test]
		public void Length()
		{
			Assert.Throws<ArgumentException>(() => new SqlTime(0).GetValue(new byte[2]));
			Assert.Throws<ArgumentException>(() => new SqlTime(0).GetValue(new byte[4]));
			Assert.Throws<ArgumentException>(() => new SqlTime(1).GetValue(new byte[2]));
			Assert.Throws<ArgumentException>(() => new SqlTime(1).GetValue(new byte[4]));
			Assert.Throws<ArgumentException>(() => new SqlTime(2).GetValue(new byte[2]));
			Assert.Throws<ArgumentException>(() => new SqlTime(2).GetValue(new byte[4]));
			Assert.Throws<ArgumentException>(() => new SqlTime(3).GetValue(new byte[3]));
			Assert.Throws<ArgumentException>(() => new SqlTime(3).GetValue(new byte[5]));
			Assert.Throws<ArgumentException>(() => new SqlTime(4).GetValue(new byte[3]));
			Assert.Throws<ArgumentException>(() => new SqlTime(4).GetValue(new byte[5]));
			Assert.Throws<ArgumentException>(() => new SqlTime(5).GetValue(new byte[4]));
			Assert.Throws<ArgumentException>(() => new SqlTime(5).GetValue(new byte[6]));
			Assert.Throws<ArgumentException>(() => new SqlTime(6).GetValue(new byte[4]));
			Assert.Throws<ArgumentException>(() => new SqlTime(6).GetValue(new byte[6]));
			Assert.Throws<ArgumentException>(() => new SqlTime(7).GetValue(new byte[4]));
			Assert.Throws<ArgumentException>(() => new SqlTime(7).GetValue(new byte[6]));
		}
	}
}