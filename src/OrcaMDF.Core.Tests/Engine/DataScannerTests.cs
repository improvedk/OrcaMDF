using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Tests.Engine
{
	public class DataScannerTests : SqlServerSystemTest
	{
		[Test]
		public void ScanUniqueClusteredTable()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("UniqueClusteredTable").ToList();

				Assert.AreEqual(112, rows[0].Field<int>("Num1"));
				Assert.AreEqual("Doe", rows[0].Field<string>("Name"));

				Assert.AreEqual(382, rows[1].Field<int>("Num1"));
				Assert.AreEqual("John", rows[1].Field<string>("Name"));
			}
		}
		
		[Test]
		public void ScanNonUniqueClusteredTable()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NonUniqueClusteredTable").ToList();

				Assert.AreEqual(112, rows[0].Field<int>("Num1"));
				Assert.AreEqual("Doe", rows[0].Field<string>("Name"));
				Assert.AreEqual(0, rows[0].Field<int>(DataColumn.Uniquifier));
				
				Assert.AreEqual(112, rows[1].Field<int>("Num1"));
				Assert.AreEqual("Doe", rows[1].Field<string>("Name"));
				Assert.AreEqual(1, rows[1].Field<int>(DataColumn.Uniquifier));
			}
		}

		[Test]
		public void ScanNonSparseInts()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("NonSparseInts").ToList();

				Assert.AreEqual(123, rows[0].Field<int?>("A"));
				Assert.AreEqual(null, rows[0].Field<int?>("B"));
				Assert.AreEqual(null, rows[0].Field<int?>("C"));
				Assert.AreEqual(127, rows[0].Field<int?>("D"));

				Assert.AreEqual(null, rows[1].Field<int?>("A"));
				Assert.AreEqual(null, rows[1].Field<int?>("B"));
				Assert.AreEqual(123982, rows[1].Field<int?>("C"));
				Assert.AreEqual(null, rows[1].Field<int?>("D"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			// Create unique clustered table
			var cmd = new SqlCommand(@"
				CREATE TABLE UniqueClusteredTable
				(
					Num1 int NOT NULL,
					Name nvarchar(30)
				)
				CREATE UNIQUE CLUSTERED INDEX CX_Num1_Name ON UniqueClusteredTable (Num1, Name)

				INSERT INTO
					UniqueClusteredTable (Num1, Name)
				VALUES
					(382, 'John'),
					(112, 'Doe')", conn);
			cmd.ExecuteNonQuery();
			
			// Create non-unique clustered table with uniquifier
			cmd = new SqlCommand(@"
				CREATE TABLE NonUniqueClusteredTable
				(
					Num1 int NOT NULL,
					Name nvarchar(30)
				)
				CREATE CLUSTERED INDEX CX_Num1_Name ON NonUniqueClusteredTable (Num1, Name)

				INSERT INTO
					NonUniqueClusteredTable (Num1, Name)
				VALUES
					(112, 'Doe'),
					(112, 'Doe')", conn);
			cmd.ExecuteNonQuery();

			// Scanning of non-sparse ints
			cmd = new SqlCommand(@"
				CREATE TABLE NonSparseInts
				(
					A int,
					B int,
					C int,
					D int
				)
				INSERT INTO
					NonSparseInts
				VALUES
					(123, NULL, NULL, 127),
					(NULL, NULL, 123982, NULL)", conn);
			cmd.ExecuteNonQuery();
		}
	}
}