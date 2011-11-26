using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.DataTypes
{
	public class MoneyTestsBase : SqlServerSystemTestBase
	{
		[SqlServerTest]
		public void MoneyTest(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("MoneyTest").ToList();

				Assert.AreEqual(123.4568m, rows[0].Field<decimal>("A"));
				Assert.AreEqual(-123.4568m, rows[1].Field<decimal>("A"));
				Assert.AreEqual(123456789.0123m, rows[2].Field<decimal>("A"));
				Assert.AreEqual(-123456789.0123m, rows[3].Field<decimal>("A"));
				Assert.AreEqual(-922337203685477.5808m, rows[4].Field<decimal>("A"));
				Assert.AreEqual(922337203685477.5807m, rows[5].Field<decimal>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE MoneyTest (A money)
				INSERT INTO MoneyTest VALUES (123.456789)
				INSERT INTO MoneyTest VALUES (-123.456789)
				INSERT INTO MoneyTest VALUES (123456789.0123)
				INSERT INTO MoneyTest VALUES (-123456789.0123)
				INSERT INTO MoneyTest VALUES (-922337203685477.5808)
				INSERT INTO MoneyTest VALUES (922337203685477.5807)
			", conn);
		}
	}
}