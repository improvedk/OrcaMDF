using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.DataTypes
{
	public class SmallMoneyTests : SqlServerSystemTest
	{
		[Test]
		public void SmallMoneyTest()
		{
			using (var db = new Database(DataFilePaths))
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("SmallMoneyTest").ToList();

				Assert.AreEqual(123.4568m, rows[0].Field<decimal>("A"));
				Assert.AreEqual(-123.4568m, rows[1].Field<decimal>("A"));
				Assert.AreEqual(123456.0123m, rows[2].Field<decimal>("A"));
				Assert.AreEqual(-123456.0123m, rows[3].Field<decimal>("A"));
				Assert.AreEqual(-214748.3648m, rows[4].Field<decimal>("A"));
				Assert.AreEqual(214748.3647m, rows[5].Field<decimal>("A"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"
				CREATE TABLE SmallMoneyTest (A smallmoney)
				INSERT INTO SmallMoneyTest VALUES (123.456789), (-123.456789), (123456.0123), (-123456.0123), (-214748.3648), (214748.3647)
			", conn);
		}
	}
}