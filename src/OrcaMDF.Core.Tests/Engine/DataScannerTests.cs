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

		protected override void RunSetupQueries(SqlConnection conn)
		{
			// Create unique clustered table
			RunQuery(@"	CREATE TABLE UniqueClusteredTable
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
			
			// Create non-unique clustered table with uniquifier
			RunQuery(@"	CREATE TABLE NonUniqueClusteredTable
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
		}
	}
}