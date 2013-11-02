using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;
using OrcaMDF.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;

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
		public void UniqueidentifierTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("UniqueidentifierTests").ToList();

				Assert.AreEqual(null, rows[0].Field<Guid?>("A"));
				Assert.AreEqual(new Guid("92F9A6D1-E99E-49AC-9D85-996F4BC08B20"), rows[1].Field<Guid?>("A"));
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

		[SqlServer2008PlusTest]
		public void ImageTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("ImageTests").ToList();

				Assert.AreEqual(null, rows[0].Field<byte[]>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("25FF25"), rows[1].Field<byte[]>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("01020304050607080910"), rows[2].Field<byte[]>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void VarbinaryTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarbinaryTests").ToList();

				Assert.AreEqual(null, rows[0].Field<byte[]>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("25FF25"), rows[1].Field<byte[]>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("01020304050607080910"), rows[2].Field<byte[]>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void MoneyTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("MoneyTests").ToList();

				Assert.AreEqual(123.4568m, rows[0].Field<decimal>("A"));
				Assert.AreEqual(-123.4568m, rows[1].Field<decimal>("A"));
				Assert.AreEqual(123456789.0123m, rows[2].Field<decimal>("A"));
				Assert.AreEqual(-123456789.0123m, rows[3].Field<decimal>("A"));
				Assert.AreEqual(-922337203685477.5808m, rows[4].Field<decimal>("A"));
				Assert.AreEqual(922337203685477.5807m, rows[5].Field<decimal>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void SmallMoneyTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("SmallMoneyTests").ToList();

				Assert.AreEqual(123.4568m, rows[0].Field<decimal>("A"));
				Assert.AreEqual(-123.4568m, rows[1].Field<decimal>("A"));
				Assert.AreEqual(123456.0123m, rows[2].Field<decimal>("A"));
				Assert.AreEqual(-123456.0123m, rows[3].Field<decimal>("A"));
				Assert.AreEqual(-214748.3648m, rows[4].Field<decimal>("A"));
				Assert.AreEqual(214748.3647m, rows[5].Field<decimal>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void NCharTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("NCharTests").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
				Assert.AreEqual("a", rows[1].Field<string>("A"));
				Assert.AreEqual("1234567890", rows[2].Field<string>("A"));
				Assert.AreEqual("123", rows[3].Field<string>("A"));
				Assert.AreEqual("", rows[4].Field<string>("A"));
				Assert.AreEqual("", rows[5].Field<string>("A"));
				Assert.AreEqual("ѨѨѨѨѨѨѨѨѨѨ", rows[6].Field<string>("A"));
			});
		}

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
				Assert.AreEqual(127, rows[10].Field<int?>("A"));
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

		[SqlServer2008PlusTest]
		public void NVarcharTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("NVarcharTests").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
				Assert.AreEqual("a", rows[1].Field<string>("A"));
				Assert.AreEqual("1234567890", rows[2].Field<string>("A"));
				Assert.AreEqual("123", rows[3].Field<string>("A"));
				Assert.AreEqual("", rows[4].Field<string>("A"));
				Assert.AreEqual(" ", rows[5].Field<string>("A"));
				Assert.AreEqual("ѨѨѨѨѨѨѨѨѨѨ", rows[6].Field<string>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void VarcharTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VarcharTests").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
				Assert.AreEqual("a", rows[1].Field<string>("A"));
				Assert.AreEqual("1234567890", rows[2].Field<string>("A"));
				Assert.AreEqual("123", rows[3].Field<string>("A"));
				Assert.AreEqual("", rows[4].Field<string>("A"));
				Assert.AreEqual(" ", rows[5].Field<string>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void NTextTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("NTextTests").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
				Assert.AreEqual("a", rows[1].Field<string>("A"));
				Assert.AreEqual("1234567890", rows[2].Field<string>("A"));
				Assert.AreEqual("ѨѨѨѨ", rows[3].Field<string>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void TextTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TextTests").ToList();

				Assert.AreEqual(null, rows[0].Field<string>("A"));
				Assert.AreEqual("a", rows[1].Field<string>("A"));
				Assert.AreEqual("1234567890", rows[2].Field<string>("A"));
			});
		}

		[SqlServer2008PlusTest]
		public void DatetimeTests(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("DatetimeTest").ToList();

				Assert.AreEqual(new DateTime(2012, 01, 29, 23, 57, 42, 997), rows[0].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(2012, 01, 29, 23, 57, 42, 447), rows[1].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(2099, 12, 31, 23, 59, 59, 997), rows[2].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1753, 01, 01, 00, 00, 00, 000), rows[3].Field<DateTime?>("A"));
				Assert.AreEqual(null, rows[4].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 00, 00, 00, 000), rows[5].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 22, 17, 21, 447), rows[6].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 05, 06, 07, 997), rows[7].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 01, 13, 12, 11, 447), rows[8].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 02, 00, 00, 00, 000), rows[9].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1900, 01, 02, 18, 22, 11, 123), rows[10].Field<DateTime?>("A"));
				Assert.AreEqual(new DateTime(1899, 01, 02, 18, 22, 11, 123), rows[11].Field<DateTime?>("A"));
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

				CREATE TABLE UniqueidentifierTests (A uniqueidentifier) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					UniqueidentifierTests
				VALUES
					(NULL),
					('92F9A6D1-E99E-49AC-9D85-996F4BC08B20')

				CREATE TABLE NCharTests (A nchar(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					NCharTests
				VALUES
					(NULL),
					('a'),
					('1234567890'),
					('123'),
					(''),
					(' '),
					(N'ѨѨѨѨѨѨѨѨѨѨ')

				CREATE TABLE NVarcharTests (A nvarchar(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					NVarcharTests
				VALUES
					(NULL),
					('a'),
					('1234567890'),
					('123'),
					(''),
					(' '),
					(N'ѨѨѨѨѨѨѨѨѨѨ')

				CREATE TABLE VarcharTests (A varchar(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					VarcharTests
				VALUES
					(NULL),
					('a'),
					('1234567890'),
					('123'),
					(''),
					(' ')

				CREATE TABLE NTextTests (A ntext) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					NTextTests
				VALUES
					(NULL),
					('a'),
					('1234567890'),
					(N'ѨѨѨѨ')

				CREATE TABLE TextTests (A text) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					TextTests
				VALUES
					(NULL),
					('a'),
					('1234567890')

				CREATE TABLE MoneyTests (A money) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					MoneyTests
				VALUES
					(123.456789),
					(-123.456789),
					(123456789.0123),
					(-123456789.0123),
					(-922337203685477.5808),
					(922337203685477.5807)

				CREATE TABLE SmallMoneyTests (A smallmoney) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					SmallMoneyTests
				VALUES
					(123.456789),
					(-123.456789),
					(123456.0123),
					(-123456.0123),
					(-214748.3648),
					(214748.3647)

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

				CREATE TABLE ImageTests (A image) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					ImageTests
				VALUES
					(NULL),
					(0x25FF25),
					(0x01020304050607080910)

				CREATE TABLE VarbinaryTests (A varbinary(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					VarbinaryTests
				VALUES
					(NULL),
					(0x25FF25),
					(0x01020304050607080910)

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
					(0),
					(127)
					
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

				CREATE TABLE DatetimeTest (A datetime) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					DatetimeTest
				VALUES
					('2012-01-29 23:57:42.997'),
					('2012-01-29 23:57:42.447'),
					('2099-12-31 23:59:59.997'),
					('1753-01-01 00:00:00.000'),
					(NULL),
					('1900-01-01 00:00:00.000'),
					('1900-01-01 22:17:21.447'),
					('1900-01-01 05:06:07.997'),
					('1900-01-01 13:12:11.447'),
					('1900-01-02 00:00:00.000'),
					('1900-01-02 18:22:11.123'),
					('1899-01-02 18:22:11.123')
				", conn);
		}
	}
}