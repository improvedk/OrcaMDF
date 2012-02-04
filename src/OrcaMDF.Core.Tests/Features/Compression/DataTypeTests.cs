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

		[SqlServer2008PlusTest]
		public void DecimalTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("DecimalTests").ToList();

				Assert.AreEqual(12.3m, rows[0].Field<decimal>("A"));
				Assert.AreEqual(0m, rows[0].Field<decimal>("B"));
				Assert.AreEqual(1m, rows[0].Field<decimal>("C"));
				Assert.AreEqual(12345m, rows[0].Field<decimal>("D"));
				Assert.AreEqual(39201.230m, rows[0].Field<decimal>("E"));
				Assert.AreEqual(-4892384.382090m, rows[0].Field<decimal>("F"));
				Assert.AreEqual(1328783742987.29m, rows[0].Field<decimal>("G"));
				Assert.AreEqual(2940382040198493029.235m, rows[0].Field<decimal>("H"));
				Assert.AreEqual(-1m, rows[0].Field<decimal>("I"));
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

				CREATE TABLE DecimalTests
				(
					A decimal(18, 4) NOT NULL,
					B decimal(8, 0) NOT NULL,
					C decimal(15, 7) NOT NULL,
					D decimal(5, 0) NOT NULL,
					E decimal(9, 3) NOT NULL,
					F decimal(14, 6) NOT NULL,
					G decimal(17, 2) NOT NULL,
					H decimal(22, 3) NOT NULL,
					I decimal(11, 3) NOT NULL
				) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO DecimalTests VALUES (12.3, 0, 1, 12345, 39201.230, -4892384.38209, 1328783742987.29, 2940382040198493029.23456, -1)
				", conn);
		}
	}
}