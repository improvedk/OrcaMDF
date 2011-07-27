using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.LOB
{
	public class ClassicLobTests : SqlServerSystemTest
	{
		[Test]
		public void SinglePageText()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("SinglePageText").ToList();

				Assert.AreEqual(28, rows[0].Field<int>("A"));
				Assert.AreEqual("".PadLeft(10, 'a'), rows[0].Field<string>("B"));
			}
		}

		[Test]
		public void ThreePageText()
		{
			
		}

		[Test]
		public void SinglePageNText()
		{
			
		}

		[Test]
		public void SinglePageImage()
		{
			
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"	CREATE TABLE SinglePageText
						(
							A int,
							B text
						)
						INSERT INTO SinglePageText VALUES (28, REPLICATE('a', 10))", conn);
		}
	}
}
