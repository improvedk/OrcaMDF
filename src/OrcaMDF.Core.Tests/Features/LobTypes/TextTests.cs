using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.LobTypes
{
	public class TextTests : SqlServerSystemTestBase
	{
		[SqlServerTest]
		public void TextNull(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTestNull").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void TextEmpty(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTestEmpty").ToList();

				Assert.AreEqual("", rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void Text64(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTest64").ToList();

				Assert.AreEqual("".PadLeft(64, 'A'), rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void Text65(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTest65").ToList();

				Assert.AreEqual("".PadLeft(65, 'A'), rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void Text8040(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTest8040").ToList();

				Assert.AreEqual("".PadLeft(8040, 'A'), rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void Text8041(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTest8041").ToList();

				Assert.AreEqual("".PadLeft(8041, 'A'), rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void Text40200(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTest40200").ToList();

				Assert.AreEqual("".PadLeft(40200, 'A'), rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void Text40201(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTest40201").ToList();

				Assert.AreEqual("".PadLeft(40201, 'A'), rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void Text20000000(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTest20000000").ToList();

				Assert.AreEqual("".PadLeft(20000000, 'A'), rows[0].Field<string>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"	CREATE TABLE TextTestNull ( A text )
						INSERT INTO TextTestNull VALUES (NULL)

						CREATE TABLE TextTestEmpty ( A text )
						INSERT INTO TextTestEmpty VALUES ('')

						CREATE TABLE TextTest64 ( A text )
						INSERT INTO TextTest64 VALUES (REPLICATE('A', 64))

						CREATE TABLE TextTest65 ( A text )
						INSERT INTO TextTest65 VALUES (REPLICATE('A', 65))

						CREATE TABLE TextTest8040 ( A text )
						INSERT INTO TextTest8040 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 8040))

						CREATE TABLE TextTest8041 ( A text )
						INSERT INTO TextTest8041 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 8041))

						CREATE TABLE TextTest40200 ( A text )
						INSERT INTO TextTest40200 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 40200))

						CREATE TABLE TextTest40201 ( A text )
						INSERT INTO TextTest40201 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 40201))

						CREATE TABLE TextTest20000000 ( A text )
						INSERT INTO TextTest20000000 VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 20000000))", conn);
		}
	}
}