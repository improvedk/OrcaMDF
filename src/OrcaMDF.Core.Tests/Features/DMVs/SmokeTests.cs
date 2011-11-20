using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.DMVs
{
	public class SmokeTests : SqlServerSystemTest
	{
		[Test]
		public void SysColumns()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.Columns.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysForeignKeys()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.ForeignKeys.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysIndexes()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.Indexes.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysIndexColumns()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.IndexColumns.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysObjects()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.Objects.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysObjectsDollar()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.ObjectsDollar.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysSystemInternalsAllocationUnit()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.SystemInternalsAllocationUnits.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysSystemInternalsPartition()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.SystemInternalsPartitions.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysSystemInternalsPartitionColumns()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.SystemInternalsPartitionColumns.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysTables()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.Tables.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		[Test]
		public void SysTypes()
		{
			using (var db = new Database(DataFilePaths))
			{
				var row = db.Dmvs.Types.First();
				TestHelper.GetAllPublicProperties(row);
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"	CREATE TABLE TestA (A int, PRIMARY KEY CLUSTERED (A))
						CREATE TABLE TestB (B int, FOREIGN KEY (B) REFERENCES TestA(A))", conn);
		}
	}
}