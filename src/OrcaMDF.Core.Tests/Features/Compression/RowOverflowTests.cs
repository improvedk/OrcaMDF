using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Compression
{
	public class RowOverflowTests : CompressionTestBase
	{
		[SqlServer2008PlusTest]
		public void VarcharOverflow(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarcharOverflow").ToList();

				Assert.AreEqual("".PadLeft(5000, 'A'), rows[0].Field<string>("A"));
				Assert.AreEqual("".PadLeft(5000, 'B'), rows[0].Field<string>("B"));
			});
		}

		[SqlServer2008PlusTest]
		public void VarcharBlobInlineRoot(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarcharBlobInlineRoot").ToList();

				Assert.AreEqual("".PadLeft(25000, 'A'), rows[0].Field<string>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE VarcharOverflow (A varchar(8000), B varchar(8000)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO VarcharOverflow VALUES (REPLICATE('A', 5000), REPLICATE('B', 5000))

				CREATE TABLE VarcharBlobInlineRoot (A varchar(MAX)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO VarcharBlobInlineRoot VALUES (REPLICATE(CAST('A' AS varchar(MAX)), 25000))
				", conn);
		}
	}
}