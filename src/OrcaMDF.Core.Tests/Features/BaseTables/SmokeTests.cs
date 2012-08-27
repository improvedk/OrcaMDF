using System.Data.SqlClient;
using System.Linq;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.BaseTables
{
	public class SmokeTestsBase : SqlServerSystemTestBase
	{
		[SqlServerTest]
		public void Sysobjvalues(DatabaseVersion version)
		{
			RunDatabaseTest(version, db => {
				var row = db.BaseTables.sysobjvalues.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE TestA (A int, PRIMARY KEY CLUSTERED (A));
				CREATE TABLE TestB (B int, FOREIGN KEY (B) REFERENCES TestA(A));
			", conn);

			RunQuery(@"
				CREATE PROCEDURE TestC AS SELECT 1 AS A;
			", conn);
		}
	}
}
