using NUnit.Framework;
using OrcaMDF.RawCore.Records;
using OrcaMDF.RawCore.Types;
using System;
using System.Linq;

namespace OrcaMDF.RawCore.Tests
{
	public class RawColumnParserTests : BaseFixture
	{
		[TestCase(AW2005Path, 118, "9.06.04.26.00", "2006-04-26", TestName = "2005")]
		[TestCase(AW2008Path, 187, "10.00.80404.00", "2008-04-04", TestName = "2008")]
		[TestCase(AW2008R2Path, 184, "10.00.80404.00", "2008-04-04", TestName = "2008R2")]
		[TestCase(AW2012Path, 187, "11.0.2100.60", "2012-03-15", TestName = "2012")]
		public void Parse_BuildVersion(string dbPath, int pageID, string databaseVersion, string versionDate)
		{
			var db = new RawDatabase(dbPath);
			var page = db.GetPage(1, pageID);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.TinyInt("SystemInformationID"),
				RawType.NVarchar("Database Version"),
				RawType.DateTime("VersionDate"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(4, result.Count);
			Assert.AreEqual(1, (byte)result["SystemInformationID"]);
			Assert.AreEqual(databaseVersion, result["Database Version"]);
			Assert.AreEqual(Convert.ToDateTime(versionDate), (DateTime)result["VersionDate"]);
			Assert.AreEqual(Convert.ToDateTime(versionDate), (DateTime)result["ModifiedDate"]);
		}

		[TestCase(AW2005Path, 356, TestName = "2005")]
		[TestCase(AW2008Path, 405, TestName = "2008")]
		[TestCase(AW2008R2Path, 197, TestName = "2008R2")]
		[TestCase(AW2012Path, 405, TestName = "2012")]
		public void Parse_Address(string dbPath, int pageID)
		{
			var db = new RawDatabase(dbPath);
			var page = db.GetPage(1, pageID);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.Int("AddressID"),
				RawType.NVarchar("AddressLine1"),
				RawType.NVarchar("AddressLine2"),
				RawType.NVarchar("City"),
				RawType.NVarchar("StateProvince"),
				RawType.NVarchar("CountryRegion"),
				RawType.NVarchar("PostalCode"),
				RawType.UniqueIdentifier("rowguid"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(9, result.Count);
			Assert.AreEqual(9, (int)result["AddressID"]);
			Assert.AreEqual("8713 Yosemite Ct.", result["AddressLine1"].ToString());
			Assert.AreEqual(null, result["AddressLine2"]);
			Assert.AreEqual("Bothell", result["City"].ToString());
			Assert.AreEqual("Washington", result["StateProvince"].ToString());
			Assert.AreEqual("United States", result["CountryRegion"].ToString());
			Assert.AreEqual("98011", result["PostalCode"].ToString());
			Assert.AreEqual(new Guid("268af621-76d7-4c78-9441-144fd139821a"), result["rowguid"]);
			Assert.AreEqual(new DateTime(2002, 07, 01), (DateTime)result["ModifiedDate"]);
		}
	}
}