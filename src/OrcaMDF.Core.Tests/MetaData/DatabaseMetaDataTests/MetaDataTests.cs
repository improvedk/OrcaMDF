using System.Data.SqlClient;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.MetaData.DatabaseMetaDataTests
{
	public class MetaDataTests : SqlServerSystemTest
	{
		[Test]
		public void ParseUserTableNames()
		{
			using(var mdf = new MdfFile(MdfPath))
			{
				var metaData = mdf.GetMetaData();

				Assert.AreEqual(2, metaData.UserTableNames.Length);
				Assert.AreEqual("MyTable", metaData.UserTableNames[0]);
				Assert.AreEqual("XYZ", metaData.UserTableNames[1]);
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			var cmd = new SqlCommand(@"
				CREATE TABLE MyTable (ID int);
				CREATE TABLE XYZ (ID int);", conn);
			cmd.ExecuteNonQuery();
		}
	}
}