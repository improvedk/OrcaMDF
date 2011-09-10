using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.LobTypes
{
	public class NTextTests : SqlServerSystemTest
	{
		[Test]
		public void NTextNull()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTestNull").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NTextEmpty()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTestEmpty").ToList();

				Assert.AreEqual("", rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NText32()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTest32").ToList();

				Assert.AreEqual("".PadLeft(32, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NText33()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTest33").ToList();

				Assert.AreEqual("".PadLeft(33, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NText4020()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTest4020").ToList();

				Assert.AreEqual("".PadLeft(4020, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NText4021()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTest4021").ToList();

				Assert.AreEqual("".PadLeft(4021, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NText20100()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTest20100").ToList();

				Assert.AreEqual("".PadLeft(20100, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NText20101()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTest20101").ToList();

				Assert.AreEqual("".PadLeft(20101, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NText10000000()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NTextTest10000000").ToList();

				Assert.AreEqual("".PadLeft(10000000, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(string.Format(@"	CREATE TABLE NTextTestNull ( A ntext )
										INSERT INTO NTextTestNull VALUES (NULL)

										CREATE TABLE NTextTestEmpty ( A ntext )
										INSERT INTO NTextTestEmpty VALUES ('')

										CREATE TABLE NTextTest32 ( A ntext )
										INSERT INTO NTextTest32 VALUES (REPLICATE(N'{0}', 32))

										CREATE TABLE NTextTest33 ( A ntext )
										INSERT INTO NTextTest33 VALUES (REPLICATE(N'{0}', 33))

										CREATE TABLE NTextTest4020 ( A ntext )
										INSERT INTO NTextTest4020 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 4020))

										CREATE TABLE NTextTest4021 ( A ntext )
										INSERT INTO NTextTest4021 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 4021))

										CREATE TABLE NTextTest20100 ( A ntext )
										INSERT INTO NTextTest20100 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 20100))

										CREATE TABLE NTextTest20101 ( A ntext )
										INSERT INTO NTextTest20101 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 20101))

										CREATE TABLE NTextTest10000000 ( A ntext )
										INSERT INTO NTextTest10000000 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 10000000))", '\u040A'), conn);
		}
	}
}