using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Engine.Records
{
	public class RecordTests : SqlServerSystemTest
	{
		[Test]
		public void RowOverflowPointer()
		{
			using (var db = new Database(DataFilePaths))
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("RowOverflowPointer").ToList();

				Assert.AreEqual(1, rows.Count);
				Assert.AreEqual("".PadLeft(5000, 'a'), rows[0]["A"]);
				Assert.AreEqual("".PadLeft(5000, 'b'), rows[0]["B"]);
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"	CREATE TABLE RowOverflowPointer (A varchar(8000), B varchar(8000))
						INSERT INTO RowOverflowPointer VALUES (REPLICATE('a', 5000), REPLICATE('b', 5000))", conn);
		}
	}
}