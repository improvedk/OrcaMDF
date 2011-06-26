using System.Linq;
using System.Data.SqlClient;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Tests.MetaData.DatabaseMetaDataTests
{
	public class ClusteredTableScanningTests : SqlServerSystemTest
	{
		private class ClusteredTableRow : DataRow
		{
			public ClusteredTableRow()
			{
				Columns.Add(new DataColumn("Num1", "int"));
				Columns.Add(new DataColumn("Name", "nvarchar(30)"));
			}
		}

		[Test]
		public void ScanUniqueClusteredTable()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable<ClusteredTableRow>("ClusteredTable").ToList();

				Assert.AreEqual(112, rows[0].Field<int>("Num1"));
				Assert.AreEqual("Doe", rows[0].Field<string>("Name"));

				Assert.AreEqual(382, rows[1].Field<int>("Num1"));
				Assert.AreEqual("John", rows[1].Field<string>("Name"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			// Create unique clustered table
			var cmd = new SqlCommand(@"
				CREATE TABLE ClusteredTable
				(
					Num1 int NOT NULL,
					Name nvarchar(30)
				)
				CREATE UNIQUE CLUSTERED INDEX CX_Num1_Name ON ClusteredTable (Num1, Name)

				INSERT INTO
					ClusteredTable (Num1, Name)
				VALUES
					(382, 'John'),
					(112, 'Doe')", conn);
			cmd.ExecuteNonQuery();
		}
	}
}