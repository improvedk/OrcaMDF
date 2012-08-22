using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.LobTypes
{
	public class VarBinaryTests : SqlServerSystemTestBase
	{
		[SqlServerTest]
		public void VarBinaryNull(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTestNull").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinaryEmpty(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTestEmpty").ToList();

				Assert.AreEqual("", rows[0].Field<byte[]>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinary64(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTest64").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(64, 'A')), rows[0].Field<byte[]>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinary65(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTest65").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(65, 'A')), rows[0].Field<byte[]>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinary8040(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTest8040").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(8040, 'A')), rows[0].Field<byte[]>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinary8041(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTest8041").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(8041, 'A')), rows[0].Field<byte[]>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinary40200(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTest40200").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(40200, 'A')), rows[0].Field<byte[]>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinary40201(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTest40201").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(40201, 'A')), rows[0].Field<byte[]>("A"));
			});
		}

		[SqlServerTest]
		public void VarBinary20000000(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarBinaryTest20000000").ToList();

				Assert.AreEqual(Encoding.UTF7.GetBytes("".PadLeft(20000000, 'A')), rows[0].Field<byte[]>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"	CREATE TABLE VarBinaryTestNull ( A varbinary(MAX) )
						INSERT INTO VarBinaryTestNull VALUES (NULL)

						CREATE TABLE VarBinaryTestEmpty ( A varbinary(MAX) )
						INSERT INTO VarBinaryTestEmpty VALUES (CAST('' AS varbinary(MAX)))

						CREATE TABLE VarBinaryTest64 ( A varbinary(MAX) )
						INSERT INTO VarBinaryTest64 VALUES (CAST(REPLICATE('A', 64) AS varbinary(MAX)))

						CREATE TABLE VarBinaryTest65 ( A varbinary(MAX) )
						INSERT INTO VarBinaryTest65 VALUES (CAST(REPLICATE('A', 65) AS varbinary(MAX)))

						CREATE TABLE VarBinaryTest8040 ( A varbinary(MAX) )
						INSERT INTO VarBinaryTest8040 VALUES (CAST(REPLICATE(CAST('A' AS varchar(MAX)), 8040) AS varbinary(MAX)))

						CREATE TABLE VarBinaryTest8041 ( A varbinary(MAX) )
						INSERT INTO VarBinaryTest8041 VALUES (CAST(REPLICATE(CAST('A' AS varchar(MAX)), 8041) AS varbinary(MAX)))

						CREATE TABLE VarBinaryTest40200 ( A varbinary(MAX) )
						INSERT INTO VarBinaryTest40200 VALUES (CAST(REPLICATE(CAST('A' AS varchar(MAX)), 40200) AS varbinary(MAX)))

						CREATE TABLE VarBinaryTest40201 ( A varbinary(MAX) )
						INSERT INTO VarBinaryTest40201 VALUES (CAST(REPLICATE(CAST('A' AS varchar(MAX)), 40201) AS varbinary(MAX)))

						CREATE TABLE VarBinaryTest20000000 ( A varbinary(MAX) )
						INSERT INTO VarBinaryTest20000000 VALUES (CAST(REPLICATE(CAST('A' AS varchar(MAX)), 20000000) AS varbinary(MAX)))", conn);
		}
	}
}