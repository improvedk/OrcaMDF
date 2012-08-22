using System;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.DataTypes
{
	public class SmallDatetimeTests : SqlServerSystemTestBase
	{
		[SqlServerTest]
        public void SmallDatetimeTest(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("SmallDatetimeTest").ToList();

				Assert.AreEqual(new DateTime(2012, 08, 07, 12, 23, 00), rows[0].Field<DateTime>("A"));
				Assert.AreEqual(new DateTime(2011, 02, 23, 01, 02, 00), rows[0].Field<DateTime>("B"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 00, 00, 00), rows[0].Field<DateTime>("C"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 00, 01, 00), rows[0].Field<DateTime>("D"));
				Assert.AreEqual(new DateTime(2079, 06, 06, 23, 59, 00), rows[0].Field<DateTime>("E"));
				Assert.AreEqual(new DateTime(2079, 06, 06, 23, 58, 00), rows[0].Field<DateTime>("F"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE SmallDatetimeTest
				(
					A smalldatetime,
					B smalldatetime,
					C smalldatetime,
					D smalldatetime,
					E smalldatetime,
					F smalldatetime
				)
    
				INSERT INTO
					SmallDatetimeTest
				VALUES (
					'2012-08-07 12:23:05',
					'2011-02-23 01:02:00',
					'1900-01-01 00:00:00',
					'1900-01-01 00:01:00',
					'2079-06-06 23:59:00',
					'2079-06-06 23:58:00'
				)", conn);
		}
	}
}