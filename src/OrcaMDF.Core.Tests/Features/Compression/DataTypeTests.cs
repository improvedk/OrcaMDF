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

		[SqlServer2008PlusTest]
		public void BitTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("BitTests").ToList();

				Assert.AreEqual(null, rows[0].Field<bool?>("A"));
				Assert.AreEqual(true, rows[1].Field<bool?>("A"));
				Assert.AreEqual(true, rows[2].Field<bool?>("A"));
				Assert.AreEqual(false, rows[3].Field<bool?>("A"));
				Assert.AreEqual(true, rows[4].Field<bool?>("A"));
				Assert.AreEqual(false, rows[5].Field<bool?>("A"));
				Assert.AreEqual(false, rows[6].Field<bool?>("A"));
				Assert.AreEqual(null, rows[7].Field<bool?>("A"));
				Assert.AreEqual(true, rows[8].Field<bool?>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void CharTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("CharTests").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
				Assert.AreEqual("a", rows[1].Field<string>("A"));
				Assert.AreEqual("1234567890", rows[2].Field<string>("A"));
				Assert.AreEqual("123", rows[3].Field<string>("A"));
				Assert.AreEqual("", rows[4].Field<string>("A"));
				Assert.AreEqual("", rows[5].Field<string>("A"));
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

				CREATE TABLE BitTests (A bit) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					BitTests
				VALUES
					(NULL),
					(1),
					(1),
					(0),
					(1),
					(0),
					(0),
					(NULL),
					(1)

				CREATE TABLE CharTests (A char(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					CharTests
				VALUES
					(NULL),
					('a'),
					('1234567890'),
					('123'),
					(''),
					(' ')
				", conn);
		}
	}
}