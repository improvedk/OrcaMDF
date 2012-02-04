using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Compression
{
	public class DataTypeTests : SqlServerSystemTestBase
	{
		[SqlServer2008PlusTest]
		public void BinaryTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("BinaryTest").ToList();

				Assert.AreEqual(null, rows[0].Field<byte[]>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("25FF2500000000000000"), rows[1].Field<byte[]>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("01020304050607080910"), rows[2].Field<byte[]>("A"));
			});
		}
		
		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE BinaryTest (A binary(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					BinaryTest
				VALUES
					(NULL),
					(0x25FF25),
					(0x01020304050607080910)
				", conn);
		}
	}
}