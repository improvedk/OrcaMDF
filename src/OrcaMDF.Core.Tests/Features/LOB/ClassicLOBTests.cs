using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests.Features.LOB
{
	public class ClassicLobTests : SqlServerSystemTest
	{
		[Test]
		public void SingleSmallRootStructure()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("SingleSmallRootStructure").ToList();

				Assert.AreEqual(28, rows[0].Field<int>("A"));
				Assert.AreEqual("".PadLeft(10, 'a'), rows[0].Field<string>("B"));
			}
		}

		[Test]
		public void SingleLargeRootYukonPointingToSingleDataStructure()
		{
			using (var mdf = new MdfFile(MdfPath))
			{
				var scanner = new DataScanner(mdf);
				var rows = scanner.ScanTable("SingleLargeRootYukonPointingToSingleDataStructure").ToList();

				Assert.AreEqual(28, rows[0].Field<int>("A"));
				Assert.AreEqual("".PadLeft(70, 'a'), rows[0].Field<string>("B"));
			}
		}

		protected override void RunSetupQueries(SqlConnection conn)
		{
			RunQuery(@"	CREATE TABLE SingleSmallRootStructure
						(
							A int,
							B text
						)
						INSERT INTO SingleSmallRootStructure VALUES (28, REPLICATE('a', 10))", conn);
			
			RunQuery(@"	CREATE TABLE SingleLargeRootYukonPointingToSingleDataStructure
						(
							A int,
							B text
						)
						INSERT INTO SingleLargeRootYukonPointingToSingleDataStructure VALUES (28, REPLICATE('a', 70))", conn);
		}
	}
}
