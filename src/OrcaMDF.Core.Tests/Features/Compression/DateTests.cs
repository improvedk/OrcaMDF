using System;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Compression
{
	public class DateTests : SqlServerSystemTestBase
	{
		[SqlServer2008PlusTest]
		public void DatetimeTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("DatetimeTest").ToList();

				Assert.AreEqual(new DateTime(2012, 01, 29, 23, 57, 42, 997), rows[0].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(2012, 01, 29, 23, 57, 42, 447), rows[1].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(2099, 12, 31, 23, 59, 59, 997), rows[2].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1753, 01, 01, 00, 00, 00, 000), rows[3].Field<DateTime?>("A"));
				Assert.AreEqual(null, rows[4].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 00, 00, 00, 000), rows[5].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 22, 17, 21, 447), rows[6].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 05, 06, 07, 997), rows[7].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 13, 12, 11, 447), rows[8].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 02, 00, 00, 00, 000), rows[9].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 02, 18, 22, 11, 123), rows[10].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1899, 01, 02, 18, 22, 11, 123), rows[11].Field<DateTime?>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE DatetimeTest (A datetime) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					DatetimeTest
				VALUES
					('2012-01-29 23:57:42.997'),
					('2012-01-29 23:57:42.447'),
					('2099-12-31 23:59:59.997'),
					('1753-01-01 00:00:00.000'),
					(NULL),
					('1900-01-01 00:00:00.000'),
					('1900-01-01 22:17:21.447'),
					('1900-01-01 05:06:07.997'),
					('1900-01-01 13:12:11.447'),
					('1900-01-02 00:00:00.000'),
					('1900-01-02 18:22:11.123'),
					('1899-01-02 18:22:11.123')
				", conn);
		}
	}
}