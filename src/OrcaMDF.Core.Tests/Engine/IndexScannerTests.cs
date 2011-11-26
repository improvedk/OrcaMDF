using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Engine
{
	public class IndexScannerTestsBase : SqlServerSystemTestBase
	{
		[SqlServerTest]
		public void ScanClusteredIndexOnUniqueClusteredTable(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new IndexScanner(db);
				var result = scanner.ScanIndex("UniqueClusteredTable", "CX_Num1_Name").ToList();

				Assert.AreEqual(112, result[0]["Num1"]);
				Assert.AreEqual("Doe", result[0]["Name"]);
				Assert.AreEqual(382, result[1]["Num1"]);
				Assert.AreEqual("John", result[1]["Name"]);
			});
		}

		[SqlServerTest]
		public void ScanNonclusteredIndexOnUniqueClusteredTable(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new IndexScanner(db);
				var result = scanner.ScanIndex("UniqueClusteredTable", "IDX_Num1").ToList();

				Assert.AreEqual(112, result[0]["Num1"]);
				Assert.AreEqual(382, result[1]["Num1"]);
			});
		}

		[SqlServerTest]
		public void ScanHeapAsIndex(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new IndexScanner(db);
				var result = scanner.ScanIndex("Heap", null).ToList();

				Assert.AreEqual(382, result[0]["Num1"]);
				Assert.AreEqual("John", result[0]["Name"]);
				Assert.AreEqual(112, result[1]["Num1"]);
				Assert.AreEqual("Doe", result[1]["Name"]);
			});
		}

		[SqlServerTest]
		public void ScanNonclusteredIndexOnNonUniqueClusteredTable(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var scanner = new IndexScanner(db);
				var result = scanner.ScanIndex("NonUniqueClusteredTable", "IDX_Num1").ToList();

				Assert.AreEqual(112, result[0]["Num1"]);
				Assert.AreEqual(0, result[0][DataColumn.Uniquifier]);
				Assert.AreEqual(382, result[1]["Num1"]);
				Assert.AreEqual(0, result[1][DataColumn.Uniquifier]);
				Assert.AreEqual(382, result[2]["Num1"]);
				Assert.AreEqual(1, result[2][DataColumn.Uniquifier]);
			});
		}

		[SqlServerTest]
		public void ScanNonclusteredIndexOnHeap(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				// Index stored in sorted order
				var scanner = new IndexScanner(db);
				var result = scanner.ScanIndex("Heap", "IDX_Num1").ToList();

				Assert.AreEqual(112, result[0]["Num1"]);
				Assert.AreEqual(1, ((SlotPointer)result[0][DataColumn.RID]).FileID);
				Assert.AreEqual(1, ((SlotPointer)result[0][DataColumn.RID]).SlotID);
				Assert.AreEqual(382, result[1]["Num1"]);
				Assert.AreEqual(1, ((SlotPointer)result[1][DataColumn.RID]).FileID);
				Assert.AreEqual(0, ((SlotPointer)result[1][DataColumn.RID]).SlotID);

				// Data stored in insertion order
				var dataScanner = new DataScanner(db);
				var dataResult = dataScanner.ScanTable("Heap").ToList();

				Assert.AreEqual(382, dataResult[0]["Num1"]);
				Assert.AreEqual(112, dataResult[1]["Num1"]);
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			// Create unique clustered table
			RunQuery(@"	CREATE TABLE UniqueClusteredTable
						(
							Num1 int NOT NULL,
							Name nvarchar(30)
						)
						CREATE UNIQUE CLUSTERED INDEX CX_Num1_Name ON UniqueClusteredTable (Num1, Name)
						CREATE NONCLUSTERED INDEX IDX_Num1 ON UniqueClusteredTable (Num1)

						INSERT INTO UniqueClusteredTable (Num1, Name) VALUES (382, 'John')
						INSERT INTO UniqueClusteredTable (Num1, Name) VALUES (112, 'Doe')", conn);

			// Create non unique clustered table
			RunQuery(@"	CREATE TABLE NonUniqueClusteredTable
						(
							Num1 int NOT NULL,
							Name nvarchar(30)
						)
						CREATE CLUSTERED INDEX CX_Num1_Name ON NonUniqueClusteredTable (Num1, Name)
						CREATE NONCLUSTERED INDEX IDX_Num1 ON NonUniqueClusteredTable (Num1)

						INSERT INTO NonUniqueClusteredTable (Num1, Name) VALUES (382, 'John')
						INSERT INTO NonUniqueClusteredTable (Num1, Name) VALUES (112, 'Doe')
						INSERT INTO NonUniqueClusteredTable (Num1, Name) VALUES (382, 'John')", conn);

			// Create heap
			RunQuery(@"	CREATE TABLE Heap
						(
							Num1 int NOT NULL,
							Name nvarchar(30)
						)
						CREATE NONCLUSTERED INDEX IDX_Num1 ON Heap (Num1)

						INSERT INTO Heap (Num1, Name) VALUES (382, 'John')
						INSERT INTO Heap (Num1, Name) VALUES (112, 'Doe')", conn);
		}
	}
}