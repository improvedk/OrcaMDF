using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.LobTypes
{
	public class ImageTests : SqlServerSystemTest
	{
		[Test]
		public void ImageNull()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTestNull").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void ImageEmpty()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTestEmpty").ToList();

				Assert.AreEqual("", rows[0].Field<byte[]>("A"));
			}
		}

		[Test]
		public void Image64()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTest64").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(64, 'A')), rows[0].Field<byte[]>("A"));
			}
		}

		[Test]
		public void Image65()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTest65").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(65, 'A')), rows[0].Field<byte[]>("A"));
			}
		}

		[Test]
		public void Image8040()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTest8040").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(8040, 'A')), rows[0].Field<byte[]>("A"));
			}
		}

		[Test]
		public void Image8041()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTest8041").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(8041, 'A')), rows[0].Field<byte[]>("A"));
			}
		}

		[Test]
		public void Image40200()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTest40200").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(40200, 'A')), rows[0].Field<byte[]>("A"));
			}
		}

		[Test]
		public void Image40201()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTest40201").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(40201, 'A')), rows[0].Field<byte[]>("A"));
			}
		}

		[Test]
		public void Image20000000()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("ImageTest20000000").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(20000000, 'A')), rows[0].Field<byte[]>("A"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"	CREATE TABLE ImageTestNull ( A image )
						INSERT INTO ImageTestNull VALUES (NULL)

						CREATE TABLE ImageTestEmpty ( A image )
						INSERT INTO ImageTestEmpty VALUES ('')

						CREATE TABLE ImageTest64 ( A image )
						INSERT INTO ImageTest64 VALUES (REPLICATE('A', 64))

						CREATE TABLE ImageTest65 ( A image )
						INSERT INTO ImageTest65 VALUES (REPLICATE('A', 65))

						CREATE TABLE ImageTest8040 ( A image )
						INSERT INTO ImageTest8040 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 8040))

						CREATE TABLE ImageTest8041 ( A image )
						INSERT INTO ImageTest8041 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 8041))

						CREATE TABLE ImageTest40200 ( A image )
						INSERT INTO ImageTest40200 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 40200))

						CREATE TABLE ImageTest40201 ( A image )
						INSERT INTO ImageTest40201 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 40201))

						CREATE TABLE ImageTest20000000 ( A image )
						INSERT INTO ImageTest20000000 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 20000000))", conn);
		}
	}
}