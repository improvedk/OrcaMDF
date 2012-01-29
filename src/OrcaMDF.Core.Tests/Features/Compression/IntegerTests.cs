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

				Assert.AreEqual(1, rows[0].Field<byte?>("A"));
				Assert.AreEqual(127, rows[1].Field<byte?>("A"));
				Assert.AreEqual(128, rows[2].Field<byte?>("A"));
				Assert.AreEqual(255, rows[3].Field<byte?>("A"));
				Assert.AreEqual(null, rows[4].Field<byte?>("A"));
				Assert.AreEqual(0, rows[5].Field<byte?>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void SmallintTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("SmallintTest").ToList();
				
				Assert.AreEqual(1, rows[0].Field<short?>("A"));
				Assert.AreEqual(-125, rows[1].Field<short?>("A"));
				Assert.AreEqual(-129, rows[2].Field<short?>("A"));
				Assert.AreEqual(-130, rows[3].Field<short?>("A"));
				Assert.AreEqual(125, rows[4].Field<short?>("A"));
				Assert.AreEqual(130, rows[5].Field<short?>("A"));
				Assert.AreEqual(-32768, rows[6].Field<short?>("A"));
				Assert.AreEqual(32767, rows[7].Field<short?>("A"));
				Assert.AreEqual(null, rows[8].Field<short?>("A"));
				Assert.AreEqual(0, rows[9].Field<short?>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void IntTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("IntTests").ToList();

				Assert.AreEqual(1, rows[0].Field<int?>("A"));
				Assert.AreEqual(-125, rows[1].Field<int?>("A"));
				Assert.AreEqual(-129, rows[2].Field<int?>("A"));
				Assert.AreEqual(-130, rows[3].Field<int?>("A"));
				Assert.AreEqual(125, rows[4].Field<int?>("A"));
				Assert.AreEqual(130, rows[5].Field<int?>("A"));
				Assert.AreEqual(-32768, rows[6].Field<int?>("A"));
				Assert.AreEqual(32767, rows[7].Field<int?>("A"));
				Assert.AreEqual(null, rows[8].Field<int?>("A"));
				Assert.AreEqual(0, rows[9].Field<int?>("A"));
				Assert.AreEqual(32768, rows[10].Field<int?>("A"));
				Assert.AreEqual(8388607, rows[11].Field<int?>("A"));
				Assert.AreEqual(2147483647, rows[12].Field<int?>("A"));
				Assert.AreEqual(-8388608, rows[13].Field<int?>("A"));
				Assert.AreEqual(-8388609, rows[14].Field<int?>("A"));
				Assert.AreEqual(-2147483648, rows[15].Field<int?>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void BigintTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("BigintTests").ToList();

				Assert.AreEqual(1, rows[0].Field<long?>("A"));
				Assert.AreEqual(-125, rows[1].Field<long?>("A"));
				Assert.AreEqual(-129, rows[2].Field<long?>("A"));
				Assert.AreEqual(-130, rows[3].Field<long?>("A"));
				Assert.AreEqual(125, rows[4].Field<long?>("A"));
				Assert.AreEqual(130, rows[5].Field<long?>("A"));
				Assert.AreEqual(-32768, rows[6].Field<long?>("A"));
				Assert.AreEqual(32767, rows[7].Field<long?>("A"));
				Assert.AreEqual(null, rows[8].Field<long?>("A"));
				Assert.AreEqual(0, rows[9].Field<long?>("A"));
				Assert.AreEqual(32768, rows[10].Field<long?>("A"));
				Assert.AreEqual(8388607, rows[11].Field<long?>("A"));
				Assert.AreEqual(2147483647, rows[12].Field<long?>("A"));
				Assert.AreEqual(-8388608, rows[13].Field<long?>("A"));
				Assert.AreEqual(-8388609, rows[14].Field<long?>("A"));
				Assert.AreEqual(-2147483648, rows[15].Field<long?>("A"));
				Assert.AreEqual(9223372036854775807, rows[16].Field<long?>("A"));
				Assert.AreEqual(36028797018963967, rows[17].Field<long?>("A"));
				Assert.AreEqual(140737488355327, rows[18].Field<long?>("A"));
				Assert.AreEqual(549755813887, rows[19].Field<long?>("A"));
				Assert.AreEqual(2147483648, rows[20].Field<long?>("A"));
				Assert.AreEqual(-9223372036854775808, rows[21].Field<long?>("A"));
				Assert.AreEqual(-36028797018963967, rows[22].Field<long?>("A"));
				Assert.AreEqual(-140737488355327, rows[23].Field<long?>("A"));
				Assert.AreEqual(-549755813887, rows[24].Field<long?>("A"));
				Assert.AreEqual(-2147483648, rows[25].Field<long?>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE TinyintTest (A tinyint) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					TinyintTest
				VALUES
					(1),
					(127),
					(128),
					(255),
					(NULL),
					(0)

				CREATE TABLE SmallintTest (A smallint) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					SmallintTest
				VALUES
					(1),
					(-125),
					(-129),
					(-130),
					(125),
					(130),
					(-32768),
					(32767),
					(NULL),
					(0)
					
				CREATE TABLE IntTests (A int) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					IntTests
				VALUES
					(1),
					(-125),
					(-129),
					(-130),
					(125),
					(130),
					(-32768),
					(32767),
					(NULL),
					(0),
					(32768),
					(8388607),
					(2147483647),
					(-8388608),
					(-8388609),
					(-2147483648)
					
				CREATE TABLE BigintTests (A bigint) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					BigintTests
				VALUES
					(1),
					(-125),
					(-129),
					(-130),
					(125),
					(130),
					(-32768),
					(32767),
					(NULL),
					(0),
					(32768),
					(8388607),
					(2147483647),
					(-8388608),
					(-8388609),
					(-2147483648),
					(9223372036854775807),
					(36028797018963967),
					(140737488355327),
					(549755813887),
					(2147483648),
					(-9223372036854775808),
					(-36028797018963967),
					(-140737488355327),
					(-549755813887),
					(-2147483648)
			", conn);
		}
	}
}