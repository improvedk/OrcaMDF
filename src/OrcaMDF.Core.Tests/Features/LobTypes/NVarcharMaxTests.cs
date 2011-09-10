using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.LobTypes
{
	public class NVarcharMaxTests : SqlServerSystemTest
	{
		[Test]
		public void NVarcharMaxNull()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTestNull").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMaxEmpty()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTestEmpty").ToList();

				Assert.AreEqual("", rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMax32()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTest32").ToList();

				Assert.AreEqual("".PadLeft(32, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMax33()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTest33").ToList();

				Assert.AreEqual("".PadLeft(33, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMax4020()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTest4020").ToList();

				Assert.AreEqual("".PadLeft(4020, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMax4021()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTest4021").ToList();

				Assert.AreEqual("".PadLeft(4021, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMax20100()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTest20100").ToList();

				Assert.AreEqual("".PadLeft(20100, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMax20101()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTest20101").ToList();

				Assert.AreEqual("".PadLeft(20101, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void NVarcharMax10000000()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NVarcharMaxTest10000000").ToList();

				Assert.AreEqual("".PadLeft(10000000, '\u040A'), rows[0].Field<string>("A"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(string.Format(@"	CREATE TABLE NVarcharMaxTestNull ( A ntext )
										INSERT INTO NVarcharMaxTestNull VALUES (NULL)

										CREATE TABLE NVarcharMaxTestEmpty ( A ntext )
										INSERT INTO NVarcharMaxTestEmpty VALUES ('')

										CREATE TABLE NVarcharMaxTest32 ( A ntext )
										INSERT INTO NVarcharMaxTest32 VALUES (REPLICATE(N'{0}', 32))

										CREATE TABLE NVarcharMaxTest33 ( A ntext )
										INSERT INTO NVarcharMaxTest33 VALUES (REPLICATE(N'{0}', 33))

										CREATE TABLE NVarcharMaxTest4020 ( A ntext )
										INSERT INTO NVarcharMaxTest4020 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 4020))

										CREATE TABLE NVarcharMaxTest4021 ( A ntext )
										INSERT INTO NVarcharMaxTest4021 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 4021))

										CREATE TABLE NVarcharMaxTest20100 ( A ntext )
										INSERT INTO NVarcharMaxTest20100 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 20100))

										CREATE TABLE NVarcharMaxTest20101 ( A ntext )
										INSERT INTO NVarcharMaxTest20101 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 20101))

										CREATE TABLE NVarcharMaxTest10000000 ( A ntext )
										INSERT INTO NVarcharMaxTest10000000 VALUES (REPLICATE(CAST(N'{0}' AS nvarchar(MAX)), 10000000))", '\u040A'), conn);
		}
	}
}