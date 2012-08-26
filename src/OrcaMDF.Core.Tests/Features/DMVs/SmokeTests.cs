using System.Data.SqlClient;
using System.Linq;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.DMVs
{
	public class SmokeTestsBase : SqlServerSystemTestBase
	{
		[SqlServerTest]
		public void SysColumns(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.Columns.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysForeignKeys(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.ForeignKeys.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysIndexes(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.Indexes.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysIndexColumns(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.IndexColumns.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysObjects(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.Objects.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysObjectsDollar(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.ObjectsDollar.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysPartition(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.Partitions.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysSystemInternalsAllocationUnit(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.SystemInternalsAllocationUnits.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysSystemInternalsPartition(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.SystemInternalsPartitions.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysSystemInternalsPartitionColumns(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.SystemInternalsPartitionColumns.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysTables(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.Tables.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysTypes(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.Types.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		[SqlServerTest]
		public void SysProcedures(DatabaseVersion version)
		{
			RunDatabaseTest(version, db =>
			{
				var row = db.Dmvs.Procedures.First();
				TestHelper.GetAllPublicProperties(row);
			});
		}

		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				CREATE TABLE TestA (A int, PRIMARY KEY CLUSTERED (A));
				CREATE TABLE TestB (B int, FOREIGN KEY (B) REFERENCES TestA(A));
				CREATE PROCEDURE TestC AS SELECT 1 AS A;
			", conn);
		}
	}
}