using System.Data.SqlClient;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Tests.MetaData.DatabaseMetaDataTests
{
	public class MetaDataTests : SqlServerSystemTest
	{
		[Test]
		public void ParseUserTableNames()
		{
			using (var db = new Database(DataFilePaths))
			{
				var metaData = db.GetMetaData();

				Assert.AreEqual(3, metaData.UserTableNames.Length);
				Assert.AreEqual("MyTable", metaData.UserTableNames[0]);
				Assert.AreEqual("XYZ", metaData.UserTableNames[1]);
				Assert.AreEqual("Sparse", metaData.UserTableNames[2]);
			}
		}

		[Test]
		public void DetectsSparseColumns()
		{
			using (var db = new Database(DataFilePaths))
			{
				var metaData = db.GetMetaData();

				var dr = metaData.GetEmptyDataRow("Sparse");

				Assert.AreEqual(2, dr.Columns.Count);

				Assert.IsFalse(dr.Columns[0].IsSparse);
				Assert.AreEqual(ColumnType.Int, dr.Columns[0].Type);

				Assert.IsTrue(dr.Columns[1].IsSparse);
				Assert.AreEqual(ColumnType.Int, dr.Columns[1].Type);
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			var cmd = new SqlCommand(@"
				CREATE TABLE MyTable (ID int);
				CREATE TABLE XYZ (ID int);", conn);
			cmd.ExecuteNonQuery();

			cmd = new SqlCommand(@"
				CREATE TABLE Sparse (A int, B int SPARSE)", conn);
			cmd.ExecuteNonQuery();
		}
	}
}