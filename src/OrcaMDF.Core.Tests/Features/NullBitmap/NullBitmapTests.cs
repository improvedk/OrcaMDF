using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.NullBitmap
{
	public class NullBitmapTests : SqlServerSystemTest
	{
		[Test]
		public void Garbage()
		{
			using (var db = new Database(DataFilePaths))
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("Garbage").ToList();

				Assert.AreEqual(5, rows[0].Field<int?>("A"));
				Assert.AreEqual(null, rows[0].Field<int?>("B"));
				Assert.AreEqual(null, rows[0].Field<int?>("C"));
				Assert.AreEqual(null, rows[0].Field<int?>("D"));
			}
		}

		[Test]
		public void Garbage2()
		{
			using (var db = new Database(DataFilePaths))
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable("Garbage2").ToList();

				Assert.AreEqual(5, rows[0].Field<int?>("A"));
				Assert.AreEqual(2, rows[0].Field<int?>("B"));
				Assert.AreEqual(null, rows[0].Field<string>("C"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			// A garbage bitmap may occur if it's added to an existing column that did not already have a null bitmap
			RunQuery(@"	CREATE TABLE Garbage (A int sparse)
						INSERT INTO Garbage VALUES (5)
						ALTER TABLE Garbage ADD B int NULL
						ALTER TABLE Garbage ADD C int NULL
						ALTER TABLE Garbage ADD D int NULL", conn);

			// Second test of garbage bitmaps
			RunQuery(@"	CREATE TABLE Garbage2 (A int sparse)
						INSERT INTO Garbage2 VALUES (5)
						ALTER TABLE Garbage2 ADD B int NULL
						UPDATE Garbage2 SET B = 2
						ALTER TABLE Garbage2 ADD C varchar(10)", conn);
		}
	}
}