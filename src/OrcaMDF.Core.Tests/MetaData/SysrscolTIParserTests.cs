using NUnit.Framework;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Tests.MetaData
{
	[TestFixture]
	public class SysrscolTIParserTests
	{
		[Test]
		public void Bigint()
		{
			var parser = new SysrscolTIParser(127);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(19, parser.Precision);
			Assert.AreEqual(8, parser.MaxLength);
			Assert.AreEqual(127, parser.TypeID);
			Assert.AreEqual(8, parser.MaxInrowLength);
		}

		[Test]
		public void Binary()
		{
			var parser = new SysrscolTIParser(12973);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(50, parser.MaxLength);
			Assert.AreEqual(173, parser.TypeID);
			Assert.AreEqual(50, parser.MaxInrowLength);
		}

		[Test]
		public void Bit()
		{
			var parser = new SysrscolTIParser(104);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(1, parser.Precision);
			Assert.AreEqual(1, parser.MaxLength);
			Assert.AreEqual(104, parser.TypeID);
			Assert.AreEqual(1, parser.MaxInrowLength);
		}

		[Test]
		public void Char()
		{
			var parser = new SysrscolTIParser(2735);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(10, parser.MaxLength);
			Assert.AreEqual(175, parser.TypeID);
			Assert.AreEqual(10, parser.MaxInrowLength);
		}

		[Test]
		public void Date()
		{
			var parser = new SysrscolTIParser(40);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(10, parser.Precision);
			Assert.AreEqual(3, parser.MaxLength);
			Assert.AreEqual(40, parser.TypeID);
			Assert.AreEqual(3, parser.MaxInrowLength);
		}

		[Test]
		public void Datetime()
		{
			var parser = new SysrscolTIParser(61);
			Assert.AreEqual(3, parser.Scale);
			Assert.AreEqual(23, parser.Precision);
			Assert.AreEqual(8, parser.MaxLength);
			Assert.AreEqual(61, parser.TypeID);
			Assert.AreEqual(8, parser.MaxInrowLength);
		}

		[Test]
		public void Datetime2()
		{
			var parser = new SysrscolTIParser(1834);
			Assert.AreEqual(7, parser.Scale);
			Assert.AreEqual(27, parser.Precision);
			Assert.AreEqual(8, parser.MaxLength);
			Assert.AreEqual(42, parser.TypeID);
			Assert.AreEqual(8, parser.MaxInrowLength);

			parser = new SysrscolTIParser(810);
			Assert.AreEqual(3, parser.Scale);
			Assert.AreEqual(23, parser.Precision);
			Assert.AreEqual(7, parser.MaxLength);
			Assert.AreEqual(42, parser.TypeID);
			Assert.AreEqual(7, parser.MaxInrowLength);
		}

		[Test]
		public void Datetimeoffset()
		{
			var parser = new SysrscolTIParser(1835);
			Assert.AreEqual(7, parser.Scale);
			Assert.AreEqual(34, parser.Precision);
			Assert.AreEqual(10, parser.MaxLength);
			Assert.AreEqual(43, parser.TypeID);
			Assert.AreEqual(10, parser.MaxInrowLength);

			parser = new SysrscolTIParser(1067);
			Assert.AreEqual(4, parser.Scale);
			Assert.AreEqual(31, parser.Precision);
			Assert.AreEqual(9, parser.MaxLength);
			Assert.AreEqual(43, parser.TypeID);
			Assert.AreEqual(9, parser.MaxInrowLength);
		}

		[Test]
		public void Decimal()
		{
			var parser = new SysrscolTIParser(330858);
			Assert.AreEqual(5, parser.Scale);
			Assert.AreEqual(12, parser.Precision);
			Assert.AreEqual(9, parser.MaxLength);
			Assert.AreEqual(106, parser.TypeID);
			Assert.AreEqual(9, parser.MaxInrowLength);

			parser = new SysrscolTIParser(396138);
			Assert.AreEqual(6, parser.Scale);
			Assert.AreEqual(11, parser.Precision);
			Assert.AreEqual(9, parser.MaxLength);
			Assert.AreEqual(106, parser.TypeID);
			Assert.AreEqual(9, parser.MaxInrowLength);
		}

		[Test]
		public void Float()
		{
			var parser = new SysrscolTIParser(62);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(53, parser.Precision);
			Assert.AreEqual(8, parser.MaxLength);
			Assert.AreEqual(62, parser.TypeID);
			Assert.AreEqual(8, parser.MaxInrowLength);
		}

		[Test]
		public void Varbinary()
		{
			var parser = new SysrscolTIParser(165);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(-1, parser.MaxLength);
			Assert.AreEqual(165, parser.TypeID);
			Assert.AreEqual(8000, parser.MaxInrowLength);

			parser = new SysrscolTIParser(228517);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(892, parser.MaxLength);
			Assert.AreEqual(165, parser.TypeID);
			Assert.AreEqual(892, parser.MaxInrowLength);
		}

		[Test]
		public void Image()
		{
			var parser = new SysrscolTIParser(4130);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(16, parser.MaxLength);
			Assert.AreEqual(34, parser.TypeID);
			Assert.AreEqual(16, parser.MaxInrowLength);
		}

		[Test]
		public void Int()
		{
			var parser = new SysrscolTIParser(56);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(10, parser.Precision);
			Assert.AreEqual(4, parser.MaxLength);
			Assert.AreEqual(56, parser.TypeID);
			Assert.AreEqual(4, parser.MaxInrowLength);
		}

		[Test]
		public void Money()
		{
			var parser = new SysrscolTIParser(60);
			Assert.AreEqual(4, parser.Scale);
			Assert.AreEqual(19, parser.Precision);
			Assert.AreEqual(8, parser.MaxLength);
			Assert.AreEqual(60, parser.TypeID);
			Assert.AreEqual(8, parser.MaxInrowLength);
		}

		[Test]
		public void Nchar()
		{
			var parser = new SysrscolTIParser(5359);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(20, parser.MaxLength);
			Assert.AreEqual(239, parser.TypeID);
			Assert.AreEqual(20, parser.MaxInrowLength);
		}

		[Test]
		public void Ntext()
		{
			var parser = new SysrscolTIParser(4195);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(16, parser.MaxLength);
			Assert.AreEqual(99, parser.TypeID);
			Assert.AreEqual(16, parser.MaxInrowLength);
		}

		[Test]
		public void Numeric()
		{
			var parser = new SysrscolTIParser(265580);
			Assert.AreEqual(4, parser.Scale);
			Assert.AreEqual(13, parser.Precision);
			Assert.AreEqual(9, parser.MaxLength);
			Assert.AreEqual(108, parser.TypeID);
			Assert.AreEqual(9, parser.MaxInrowLength);

			parser = new SysrscolTIParser(135020);
			Assert.AreEqual(2, parser.Scale);
			Assert.AreEqual(15, parser.Precision);
			Assert.AreEqual(9, parser.MaxLength);
			Assert.AreEqual(108, parser.TypeID);
			Assert.AreEqual(9, parser.MaxInrowLength);
		}

		[Test]
		public void Nvarchar()
		{
			var parser = new SysrscolTIParser(25831);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(100, parser.MaxLength);
			Assert.AreEqual(231, parser.TypeID);
			Assert.AreEqual(100, parser.MaxInrowLength);

			parser = new SysrscolTIParser(231);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(-1, parser.MaxLength);
			Assert.AreEqual(231, parser.TypeID);
			Assert.AreEqual(8000, parser.MaxInrowLength);
		}

		[Test]
		public void Real()
		{
			var parser = new SysrscolTIParser(59);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(24, parser.Precision);
			Assert.AreEqual(4, parser.MaxLength);
			Assert.AreEqual(59, parser.TypeID);
			Assert.AreEqual(4, parser.MaxInrowLength);
		}

		[Test]
		public void Smalldatetime()
		{
			var parser = new SysrscolTIParser(58);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(16, parser.Precision);
			Assert.AreEqual(4, parser.MaxLength);
			Assert.AreEqual(58, parser.TypeID);
			Assert.AreEqual(4, parser.MaxInrowLength);
		}

		[Test]
		public void Smallint()
		{
			var parser = new SysrscolTIParser(52);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(5, parser.Precision);
			Assert.AreEqual(2, parser.MaxLength);
			Assert.AreEqual(52, parser.TypeID);
			Assert.AreEqual(2, parser.MaxInrowLength);
		}

		[Test]
		public void Smallmoney()
		{
			var parser = new SysrscolTIParser(122);
			Assert.AreEqual(4, parser.Scale);
			Assert.AreEqual(10, parser.Precision);
			Assert.AreEqual(4, parser.MaxLength);
			Assert.AreEqual(122, parser.TypeID);
			Assert.AreEqual(4, parser.MaxInrowLength);
		}

		[Test]
		public void Sql_Variant()
		{
			var parser = new SysrscolTIParser(98);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(8016, parser.MaxLength);
			Assert.AreEqual(98, parser.TypeID);
			Assert.AreEqual(8016, parser.MaxInrowLength);
		}

		[Test]
		public void Text()
		{
			var parser = new SysrscolTIParser(4131);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(16, parser.MaxLength);
			Assert.AreEqual(35, parser.TypeID);
			Assert.AreEqual(16, parser.MaxInrowLength);
		}

		[Test]
		public void Time()
		{
			var parser = new SysrscolTIParser(1833);
			Assert.AreEqual(7, parser.Scale);
			Assert.AreEqual(16, parser.Precision);
			Assert.AreEqual(5, parser.MaxLength);
			Assert.AreEqual(41, parser.TypeID);
			Assert.AreEqual(5, parser.MaxInrowLength);

			parser = new SysrscolTIParser(1065);
			Assert.AreEqual(4, parser.Scale);
			Assert.AreEqual(13, parser.Precision);
			Assert.AreEqual(4, parser.MaxLength);
			Assert.AreEqual(41, parser.TypeID);
			Assert.AreEqual(4, parser.MaxInrowLength);
		}

		[Test]
		public void Timestamp()
		{
			var parser = new SysrscolTIParser(189);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(8, parser.MaxLength);
			Assert.AreEqual(189, parser.TypeID);
			Assert.AreEqual(8, parser.MaxInrowLength);
		}

		[Test]
		public void Tinyint()
		{
			var parser = new SysrscolTIParser(48);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(3, parser.Precision);
			Assert.AreEqual(1, parser.MaxLength);
			Assert.AreEqual(48, parser.TypeID);
			Assert.AreEqual(1, parser.MaxInrowLength);
		}

		[Test]
		public void Uniqueidentifier()
		{
			var parser = new SysrscolTIParser(36);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(16, parser.MaxLength);
			Assert.AreEqual(36, parser.TypeID);
			Assert.AreEqual(16, parser.MaxInrowLength);
		}

		[Test]
		public void Varchar()
		{
			var parser = new SysrscolTIParser(12967);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(50, parser.MaxLength);
			Assert.AreEqual(167, parser.TypeID);
			Assert.AreEqual(50, parser.MaxInrowLength);

			parser = new SysrscolTIParser(167);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(-1, parser.MaxLength);
			Assert.AreEqual(167, parser.TypeID);
			Assert.AreEqual(8000, parser.MaxInrowLength);
		}

		[Test]
		public void Xml()
		{
			var parser = new SysrscolTIParser(241);
			Assert.AreEqual(0, parser.Scale);
			Assert.AreEqual(0, parser.Precision);
			Assert.AreEqual(-1, parser.MaxLength);
			Assert.AreEqual(241, parser.TypeID);
			Assert.AreEqual(8000, parser.MaxInrowLength);
		}
	}
}