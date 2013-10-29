using NUnit.Framework;
using OrcaMDF.RawCore.Records;
using OrcaMDF.RawCore.Types;
using System;
using System.Linq;

namespace OrcaMDF.RawCore.Tests
{
	public class RawColumnParserTests : BaseFixture
	{
		[Test]
		public void Parse_AWBuildVersion()
		{
			var db = new RawDatabase(AWPath);
			var page = db.GetPage(1, 281);
			var record = page.Records.First() as RawPrimaryRecord;

			var schema = new IRawType[] {
				RawType.Tinyint("SystemInformationID"),
				RawType.NVarchar("Database Version"),
				RawType.Datetime("VersionDate"),
				RawType.Datetime("ModifiedDate")
			};

			var result = RawColumnParser.Parse(record.FixedLengthData, record.VariableLengthOffsetValues, schema);

			Assert.AreEqual(4, result.Count);
			Assert.AreEqual(1, (byte)result["SystemInformationID"]);
			Assert.AreEqual("10.50.91013.00", result["Database Version"]);
			Assert.AreEqual(new DateTime(2009, 10, 13), (DateTime)result["VersionDate"]);
			Assert.AreEqual(new DateTime(2009, 10, 13), (DateTime)result["ModifiedDate"]);
		}
	}
}