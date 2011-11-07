using System.Data.SqlClient;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Tests.MetaData.DatabaseMetaDataTests
{
	public class MetaDataTests : SqlServerSystemTest
	{
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
				CREATE TABLE Sparse (A int, B int SPARSE)", conn);
			cmd.ExecuteNonQuery();
		}
	}
}