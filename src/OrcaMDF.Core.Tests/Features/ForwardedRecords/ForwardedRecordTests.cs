using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.ForwardedRecords
{
	public class ForwardedRecordTests : SqlServerSystemTest
	{
		[Test]
		public void HeapForwardedRecord()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("HeapForwardedRecord").ToList();

				Assert.AreEqual(25, rows[0].Field<int>("A"));
				Assert.AreEqual("".PadLeft(5000, 'A'), rows[0].Field<string>("B"));

				Assert.AreEqual(28, rows[1].Field<int>("A"));
				Assert.AreEqual("".PadLeft(4000, 'B'), rows[1].Field<string>("B"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"	CREATE TABLE HeapForwardedRecord (A int, B varchar(5000))
						INSERT INTO HeapForwardedRecord VALUES (25, REPLICATE('A', 4000)), (28, REPLICATE('B', 4000))
						UPDATE HeapForwardedRecord SET B = REPLICATE('A', 5000) WHERE A = 25", conn);
		}
	}
}