using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Compression
{
	public class UniquifierTests : SqlServerSystemTestBase
	{
		[SqlServer2008PlusTest]
		public void UniquifierTest(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("UniquifierTest").ToList();

				Assert.AreEqual(1, rows[0].Field<int?>("A"));
				Assert.AreEqual(0, rows[0].Field<int?>(DataColumn.Uniquifier));
				Assert.AreEqual(1, rows[1].Field<int?>("A"));
				Assert.AreEqual(1, rows[1].Field<int?>(DataColumn.Uniquifier));
				Assert.AreEqual(1, rows[2].Field<int?>("A"));
				Assert.AreEqual(2, rows[2].Field<int?>(DataColumn.Uniquifier));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE UniquifierTest (A int) WITH (DATA_COMPRESSION = ROW)
				CREATE CLUSTERED INDEX CX_A ON UniquifierTest (A ASC)
				INSERT INTO UniquifierTest VALUES (1), (1), (1)
				", conn);
		}
	}
}