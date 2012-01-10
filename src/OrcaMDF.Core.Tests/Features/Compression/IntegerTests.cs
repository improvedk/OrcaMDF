using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Compression
{
	public class IntegerTests : SqlServerSystemTestBase
	{
		[SqlServer2008PlusTest]
		public void TinyintTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TinyintTest").ToList();

				Assert.AreEqual(1, rows[0].Field<byte>("A"));
				Assert.AreEqual(127, rows[1].Field<byte>("A"));
				Assert.AreEqual(128, rows[2].Field<byte>("A"));
				Assert.AreEqual(255, rows[3].Field<byte>("A"));
				Assert.AreEqual(null, rows[4].Field<byte?>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE TinyintTest (A tinyint) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO TinyintTest VALUES (1)
				INSERT INTO TinyintTest VALUES (127)
				INSERT INTO TinyintTest VALUES (128)
				INSERT INTO TinyintTest VALUES (255)
				INSERT INTO TinyintTest VALUES (NULL)
			", conn);
		}
	}
}