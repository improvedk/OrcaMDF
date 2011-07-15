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
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("RowOverflowPointer").ToList();

				Assert.AreEqual(1, rows.Count);
				Assert.AreEqual("".PadLeft(5000, 'a'), rows[0]["A"]);
				Assert.AreEqual("".PadLeft(5000, 'b'), rows[0]["B"]);
			}
		}

		/// <summary>
		/// TODO: We don't yet support (MAX) LOBs so this test will fail for now.
		/// </summary>
		[Test]
		public void MaxLOBSupport()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("MaxLOBSupport").ToList();

				Assert.AreEqual(1, rows.Count);
				Assert.AreEqual("".PadLeft(16000, 'a'), rows[0]["A"]);
			}
		}

		/// <summary>
		/// TODO: We don't yet support LOBs so this test will fail for now.
		/// </summary>
		[Test]
		public void LobPointer()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("LobPointer").ToList();

				Assert.AreEqual(1, rows.Count);
				Assert.AreEqual("".PadLeft(5000, 'a'), rows[0]["A"]);
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			var cmd = new SqlCommand(@"
				CREATE TABLE RowOverflowPointer (A varchar(8000), B varchar(8000))
				INSERT INTO RowOverflowPointer VALUES (REPLICATE('a', 5000), REPLICATE('b', 5000))", conn);
			cmd.ExecuteNonQuery();

			cmd = new SqlCommand(@"
				CREATE TABLE LobPointer (A text)
				INSERT INTO LobPointer VALUES (REPLICATE('a', 5000))", conn);
			cmd.ExecuteNonQuery();

			cmd = new SqlCommand(@"
				CREATE TABLE MaxLOBSupport (A varchar(MAX))
				INSERT INTO MaxLOBSupport VALUES (REPLICATE(CAST('a' AS varchar(MAX)), 8000))", conn);
			cmd.ExecuteNonQuery();
		}
	}
}