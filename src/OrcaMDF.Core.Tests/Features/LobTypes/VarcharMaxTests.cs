using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.LobTypes
{
	public class VarcharMaxTests : SqlServerSystemTest
	{
		[Test]
		public void VarcharMaxNull()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTestNull").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMaxEmpty()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTestEmpty").ToList();

				Assert.AreEqual("", rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMax64()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTest64").ToList();

				Assert.AreEqual("".PadLeft(64, 'A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMax65()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTest65").ToList();

				Assert.AreEqual("".PadLeft(65, 'A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMax8040()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTest8040").ToList();

				Assert.AreEqual("".PadLeft(8040, 'A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMax8041()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTest8041").ToList();

				Assert.AreEqual("".PadLeft(8041, 'A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMax40200()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTest40200").ToList();

				Assert.AreEqual("".PadLeft(40200, 'A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMax40201()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTest40201").ToList();

				Assert.AreEqual("".PadLeft(40201, 'A'), rows[0].Field<string>("A"));
			}
		}

		[Test]
		public void VarcharMax20000000()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("VarcharMaxTest20000000").ToList();

				Assert.AreEqual("".PadLeft(20000000, 'A'), rows[0].Field<string>("A"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"	CREATE TABLE VarcharMaxTestNull ( A varchar(MAX) )
						INSERT INTO VarcharMaxTestNull VALUES (NULL)

						CREATE TABLE VarcharMaxTestEmpty ( A varchar(MAX) )
						INSERT INTO VarcharMaxTestEmpty VALUES ('')

						CREATE TABLE VarcharMaxTest64 ( A varchar(MAX) )
						INSERT INTO VarcharMaxTest64 VALUES (REPLICATE('A', 64))

						CREATE TABLE VarcharMaxTest65 ( A varchar(MAX) )
						INSERT INTO VarcharMaxTest65 VALUES (REPLICATE('A', 65))

						CREATE TABLE VarcharMaxTest8040 ( A varchar(MAX) )
						INSERT INTO VarcharMaxTest8040 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 8040))

						CREATE TABLE VarcharMaxTest8041 ( A varchar(MAX) )
						INSERT INTO VarcharMaxTest8041 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 8041))

						CREATE TABLE VarcharMaxTest40200 ( A varchar(MAX) )
						INSERT INTO VarcharMaxTest40200 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 40200))

						CREATE TABLE VarcharMaxTest40201 ( A varchar(MAX) )
						INSERT INTO VarcharMaxTest40201 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 40201))

						CREATE TABLE VarcharMaxTest20000000 ( A varchar(MAX) )
						INSERT INTO VarcharMaxTest20000000 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 20000000))", conn);
		}
	}
}