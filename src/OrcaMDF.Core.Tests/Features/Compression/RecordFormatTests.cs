using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;
using OrcaMDF.Framework;
using System.Data.SqlClient;
using System.Linq;

namespace OrcaMDF.Core.Tests.Features.Compression
{
	public class RecordFormatTests : SqlServerSystemTestBase
	{
		[SqlServer2008PlusTest]
		public void MultipleLongDataColumns(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("MultipleLongDataColumns").ToList();

				Assert.AreEqual(TestHelper.GetBytesFromByteString("01020304050607080910"), rows[0].Field<byte[]>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("09080706050403020100"), rows[0].Field<byte[]>("B"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("112233445566778899AA"), rows[0].Field<byte[]>("C"));
			});
		}

		[SqlServer2008PlusTest]
		public void MultipleShortDataColumns(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("MultipleShortDataColumns").ToList();

				Assert.AreEqual(1, rows[0].Field<byte>("A"));
				Assert.AreEqual(2, rows[0].Field<byte>("B"));
				Assert.AreEqual(3, rows[0].Field<byte>("C"));
			});
		}

		[SqlServer2008PlusTest]
		public void MixedShortAndLongDataColumns(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("MixedShortAndLongDataColumns").ToList();

				Assert.AreEqual(8, rows[0].Field<byte>("A"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("01020304050607080910"), rows[0].Field<byte[]>("B"));
				Assert.AreEqual(9, rows[0].Field<byte>("C"));
				Assert.AreEqual(TestHelper.GetBytesFromByteString("112233445566778899AA"), rows[0].Field<byte[]>("D"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE MultipleLongDataColumns (A binary(10), B binary(10), C binary(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					MultipleLongDataColumns
				VALUES
					(0x01020304050607080910, 0x090807060504030201, 0x112233445566778899AA)

				CREATE TABLE MultipleShortDataColumns (A tinyint, B tinyint, C tinyint) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					MultipleShortDataColumns
				VALUES
					(1, 2, 3)

				CREATE TABLE MixedShortAndLongDataColumns (A tinyint, B binary(10), C tinyint, D binary(10)) WITH (DATA_COMPRESSION = ROW)
				INSERT INTO
					MixedShortAndLongDataColumns
				VALUES
					(8, 0x01020304050607080910, 9, 0x112233445566778899AA)
				", conn);
		}
	}
}