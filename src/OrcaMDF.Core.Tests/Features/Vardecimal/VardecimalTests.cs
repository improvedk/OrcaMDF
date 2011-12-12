using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Vardecimal
{
	public class VardecimalTests : SqlServerSystemTestBase
	{
		[SqlServer2005PlusTest]
		public void VardecimalTest(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("VardecimalTest").ToList();

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

		[SqlServer2005PlusTest]
		public void TruncatedZeroes(DatabaseVersion version)
		{
			// 4398046511104 = 0b1000000000000000000000000000000000000000000
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("TruncatedZeroes").ToList();

				Assert.AreEqual(4398046511104m, rows[0].Field<decimal>("A"));
				Assert.AreEqual(4398046511104m, rows[0].Field<decimal>("B"));
				Assert.AreEqual(4398046511104m, rows[0].Field<decimal>("C"));
			});
		}

		[SqlServer2005PlusTest]
		public void EnabledBeforeInserting(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("EnabledBeforeInserting").ToList();

				Assert.AreEqual(1234.45m, rows[0].Field<decimal>("A"));
			});
		}

		[SqlServer2005PlusTest]
		public void EnabledAfterInserting(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("EnabledAfterInserting").ToList();

				Assert.AreEqual(1234.45m, rows[0].Field<decimal>("A"));
			});
		}

		[SqlServer2005PlusTest]
		public void EnabledBeforeInsertingThenDisabled(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("EnabledBeforeInsertingThenDisabled").ToList();

				Assert.AreEqual(1234.45m, rows[0].Field<decimal>("A"));
			});
		}

		[SqlServer2005PlusTest]
		public void EnabledAfterInsertingThenDisabled(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("EnabledAfterInsertingThenDisabled").ToList();

				Assert.AreEqual(1234.45m, rows[0].Field<decimal>("A"));
			});
		}

		[SqlServer2005PlusTest]
		public void Nulls(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("Nulls").ToList();

				Assert.AreEqual(null, rows[0].Field<decimal?>("A"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE VardecimalTest
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
				)
				EXEC sp_tableoption 'VardecimalTest', 'vardecimal storage format', 'on'
				INSERT INTO VardecimalTest VALUES (12.3, 0, 1, 12345, 39201.230, -4892384.38209, 1328783742987.29, 2940382040198493029.23456, -1)
			", conn);

			RunQuery(@"
				CREATE TABLE EnabledBeforeInserting (A decimal(15, 2) NOT NULL)
				EXEC sp_tableoption 'EnabledBeforeInserting', 'vardecimal storage format', 'on'
				INSERT INTO EnabledBeforeInserting VALUES (1234.45)
			", conn);

			RunQuery(@"
				CREATE TABLE EnabledAfterInserting (A decimal(15, 2) NOT NULL)
				INSERT INTO EnabledAfterInserting VALUES (1234.45)
				EXEC sp_tableoption 'EnabledAfterInserting', 'vardecimal storage format', 'on'
			", conn);

			RunQuery(@"
				CREATE TABLE EnabledBeforeInsertingThenDisabled (A decimal(15, 2) NOT NULL)
				EXEC sp_tableoption 'EnabledBeforeInsertingThenDisabled', 'vardecimal storage format', 'on'
				INSERT INTO EnabledBeforeInsertingThenDisabled VALUES (1234.45)
				EXEC sp_tableoption 'EnabledBeforeInsertingThenDisabled', 'vardecimal storage format', 'off'
			", conn);

			RunQuery(@"
				CREATE TABLE EnabledAfterInsertingThenDisabled (A decimal(15, 2) NOT NULL)
				INSERT INTO EnabledAfterInsertingThenDisabled VALUES (1234.45)
				EXEC sp_tableoption 'EnabledAfterInsertingThenDisabled', 'vardecimal storage format', 'on'
				EXEC sp_tableoption 'EnabledAfterInsertingThenDisabled', 'vardecimal storage format', 'off'
			", conn);

			RunQuery(@"
				CREATE TABLE Nulls (A decimal(15, 2) NULL)
				EXEC sp_tableoption 'Nulls', 'vardecimal storage format', 'on'
				INSERT INTO Nulls VALUES (NULL)
			", conn);

			RunQuery(@"
				CREATE TABLE TruncatedZeroes (A decimal(30, 0) NOT NULL, B decimal(30, 5) NOT NULL, C decimal(30, 15) NOT NULL)
				EXEC sp_tableoption 'TruncatedZeroes', 'vardecimal storage format', 'on'
				INSERT INTO TruncatedZeroes VALUES (4398046511104, 4398046511104, 4398046511104)
			", conn);
		}
	}
}