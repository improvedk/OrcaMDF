using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Engine
{
	public class IndexScannerTests : SqlServerSystemTest
	{
		[Test]
		public void ScanClusteredIndexOnUniqueClusteredTable()
		{
			
		}

		[Test]
		public void ScanNonclusteredIndexOnUniqueClusteredTable()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new IndexScanner(mdf);
				var result = scanner.ScanClusteredTableIndex("UniqueClusteredTable", "IDX_Num1").ToList();

				Assert.AreEqual(112, result[0]["Num1"]);
				Assert.AreEqual(382, result[1]["Num1"]);
			}
		}

		[Test]
		public void ScanNonclusteredIndexOnNonUniqueClusteredTable()
		{
			
		}

		[Test]
		public void ScanNonclusteredIndexOnHeap()
		{
			
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
				CREATE NONCLUSTERED INDEX IDX_Num1 ON UniqueClusteredTable (Num1)

				INSERT INTO
					UniqueClusteredTable (Num1, Name)
				VALUES
					(382, 'John'),
					(112, 'Doe')", conn);
			cmd.ExecuteNonQuery();
		}
	}
}