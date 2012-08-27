using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.SparseColumns
{
	public class SparseColumnTests : SqlServerSystemTestBase
	{
		[SqlServer2008PlusTest]
		public void ScanNonSparseInts(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("ScanNonSparseInts").ToList();

				Assert.AreEqual(123, rows[0].Field<int?>("A"));
				Assert.AreEqual(null, rows[0].Field<int?>("B"));
				Assert.AreEqual(null, rows[0].Field<int?>("C"));
				Assert.AreEqual(127, rows[0].Field<int?>("D"));

				Assert.AreEqual(null, rows[1].Field<int?>("A"));
				Assert.AreEqual(null, rows[1].Field<int?>("B"));
				Assert.AreEqual(123982, rows[1].Field<int?>("C"));
				Assert.AreEqual(null, rows[1].Field<int?>("D"));
			});
		}

		[SqlServer2008PlusTest]
		public void ScanSparseInts(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("ScanSparseInts").ToList();

				Assert.AreEqual(1, rows[0].Field<int>("ID"));
				Assert.AreEqual(null, rows[0].Field<int?>("A"));
				Assert.AreEqual(3, rows[0].Field<int?>("B"));
				Assert.AreEqual(null, rows[0].Field<int?>("C"));
				Assert.AreEqual(null, rows[0].Field<int?>("D"));
				Assert.AreEqual(1234, rows[0].Field<int?>("E"));

				Assert.AreEqual(45, rows[1].Field<int>("ID"));
				Assert.AreEqual(243, rows[1].Field<int?>("A"));
				Assert.AreEqual(328, rows[1].Field<int?>("B"));
				Assert.AreEqual(null, rows[1].Field<int?>("C"));
				Assert.AreEqual(null, rows[1].Field<int?>("D"));
				Assert.AreEqual(null, rows[1].Field<int?>("E"));
			});
		}

		[SqlServer2008PlusTest]
		public void ScanSparseColumns(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("ScanSparseColumns").ToList();

				Assert.AreEqual(null, rows[0].Field<int?>("A"));
				Assert.AreEqual("Mark", rows[0].Field<string>("B"));
				Assert.AreEqual(3, rows[0].Field<long?>("C"));
				Assert.AreEqual(null, rows[0].Field<byte?>("D"));
				Assert.AreEqual(1234, rows[0].Field<int?>("E"));

				Assert.AreEqual(45, rows[1].Field<int?>("A"));
				Assert.AreEqual(null, rows[1].Field<string>("B"));
				Assert.AreEqual(null, rows[1].Field<long?>("C"));
				Assert.AreEqual(243, rows[1].Field<byte?>("D"));
				Assert.AreEqual(null, rows[1].Field<int?>("E"));
			});
		}

		[SqlServer2008PlusTest]
		public void ScanAllNullSparse(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("ScanAllNullSparse").ToList();

				Assert.AreEqual(null, rows[0].Field<int?>("A"));
				Assert.AreEqual(null, rows[0].Field<int?>("B"));
			});
		}

		[SqlServer2008PlusTest]
		public void ScanRecordWithoutSparseVector(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("ScanRecordWithoutSparseVector").ToList();

				Assert.AreEqual(null, rows[0].Field<int?>("A"));
				Assert.AreEqual("xyz", rows[0].Field<string>("B"));

				Assert.AreEqual(null, rows[1].Field<int?>("A"));
				Assert.AreEqual(null, rows[1].Field<string>("B"));
			});
		}

		[SqlServer2008PlusTest]
		public void DifferingRecordFormats(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("DifferingRecordFormats").ToList();

				Assert.AreEqual(5, rows[0].Field<int?>("A"));
				Assert.AreEqual(2, rows[0].Field<int?>("B"));
				Assert.AreEqual(6, rows[1].Field<int?>("A"));
				Assert.AreEqual(null, rows[1].Field<int?>("B"));
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			// Scanning of records with differing record formats
			RunQuery(@"	CREATE TABLE DifferingRecordFormats (A int SPARSE)
						INSERT INTO DifferingRecordFormats VALUES (5), (6)
						ALTER TABLE DifferingRecordFormats ADD B int NULL
						UPDATE DifferingRecordFormats SET B = 2 WHERE A = 5", conn);
			
			// Scanning of records with no sparse vector
			RunQuery(@"	CREATE TABLE ScanRecordWithoutSparseVector
						(
							A int SPARSE,
							B varchar(10)
						)
						INSERT INTO ScanRecordWithoutSparseVector (B) VALUES ('xyz'), (NULL)", conn);

			// Scanning of all-sparse tables with no values
			RunQuery(@"	CREATE TABLE ScanAllNullSparse
						(
							A int SPARSE,
							B int SPARSE
						)
						INSERT INTO ScanAllNullSparse DEFAULT VALUES", conn);

			// Scanning of non-sparse ints
			RunQuery(@"	CREATE TABLE ScanNonSparseInts
						(
							A int,
							B int,
							C int,
							D int
						)
						INSERT INTO
							ScanNonSparseInts
						VALUES
							(123, NULL, NULL, 127),
							(NULL, NULL, 123982, NULL)", conn);

			// Scanning of sparse ints
			RunQuery(@"	CREATE TABLE ScanSparseInts
						(
							ID int NOT NULL,
							A int SPARSE,
							B int SPARSE,
							C int SPARSE,
							D int SPARSE,
							E int SPARSE
						)
						INSERT INTO ScanSparseInts (ID, B, E) VALUES (1, 3, 1234)
						INSERT INTO ScanSparseInts (ID, A, B) VALUES (45, 243, 328)", conn);

			// Scanning of sparse columns
			RunQuery(@"	CREATE TABLE ScanSparseColumns
						(
							A int SPARSE,
							B varchar(10) SPARSE,
							C bigint SPARSE,
							D tinyint SPARSE,
							E int SPARSE
						)
						INSERT INTO ScanSparseColumns (B, C, E) VALUES ('Mark', 3, 1234)
						INSERT INTO ScanSparseColumns (A, D) VALUES (45, 243)", conn);
		}
	}
}