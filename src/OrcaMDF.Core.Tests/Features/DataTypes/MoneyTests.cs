using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.DataTypes
{
	public class MoneyTests : SqlServerSystemTest
	{
		[Test]
		public void MoneyTest()
		{
			using (var db = new Database(DataFilePaths))
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("MoneyTest").ToList();

				Assert.AreEqual(123.4568m, rows[0].Field<decimal>("A"));
				Assert.AreEqual(-123.4568m, rows[1].Field<decimal>("A"));
				Assert.AreEqual(123456789.0123m, rows[2].Field<decimal>("A"));
				Assert.AreEqual(-123456789.0123m, rows[3].Field<decimal>("A"));
				Assert.AreEqual(-922337203685477.5808m, rows[4].Field<decimal>("A"));
				Assert.AreEqual(922337203685477.5807m, rows[5].Field<decimal>("A"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"
				CREATE TABLE MoneyTest (A money)
				INSERT INTO MoneyTest VALUES (123.456789), (-123.456789), (123456789.0123), (-123456789.0123), (-922337203685477.5808), (922337203685477.5807)
			", conn);
		}
	}
}