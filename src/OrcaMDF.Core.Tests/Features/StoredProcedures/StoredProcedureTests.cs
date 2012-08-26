using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.StoredProcedures
{
	public class StoredProcedureTests : SqlServerSystemTestBase
	{
		[SqlServer2005PlusTest]
		public void Dmv(DatabaseVersion version)
		{
			RunDatabaseTest(version, db => {
				var procedures = db.Dmvs.Procedures;

				Assert.AreEqual(1, procedures.Count());
				Assert.AreEqual("TestA", procedures.First().Name);
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE PROCEDURE TestA AS SELECT 1 AS A;
			", conn);
		}
	}
}