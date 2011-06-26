using System;
using System.Linq;
using System.Data.SqlClient;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Tests.Engine
{
	public class ClusteredTableScanningTests : SqlServerSystemTest
	{
		[Test]
		public void ScanUniqueClusteredTable()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable<UniqueClusteredTableRow>("UniqueClusteredTable").ToList();

				Assert.AreEqual(112, rows[0].Field<int>("Num1"));
				Assert.AreEqual("Doe", rows[0].Field<string>("Name"));

				Assert.AreEqual(382, rows[1].Field<int>("Num1"));
				Assert.AreEqual("John", rows[1].Field<string>("Name"));
			}
		}

		private class UniqueClusteredTableRow : DataRow
		{
			public UniqueClusteredTableRow()
			{
				Columns.Add(new DataColumn("Num1", "int"));
				Columns.Add(new DataColumn("Name", "nvarchar(30)"));
			}
		}

		[Test]
		public void ScanNonUniqueClusteredTable()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable<NonUniqueClusteredTableRow>("NonUniqueClusteredTable").ToList();

				Assert.AreEqual(112, rows[0].Field<int>("Num1"));
				Assert.AreEqual("Doe", rows[0].Field<string>("Name"));
				//Assert.AreEqual(0, BitConverter.ToInt32(rows[0].Field<byte[]>("Uniquifier"), 0));
				
				Assert.AreEqual(112, rows[1].Field<int>("Num1"));
				Assert.AreEqual("Doe", rows[1].Field<string>("Name"));
				Assert.AreEqual(1, BitConverter.ToInt32(rows[1].Field<byte[]>("Uniquifier"), 0));
			}
		}

		private class NonUniqueClusteredTableRow : DataRow
		{
			public NonUniqueClusteredTableRow()
			{
				Columns.Add(new DataColumn("Num1", "int"));
				Columns.Add(new DataColumn("Uniquifier", "varbinary(4)"));
				Columns.Add(new DataColumn("Name", "nvarchar(30)"));
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
		}
	}
}