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

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.TinyInt("SystemInformationID"),
				RawType.NVarchar("Database Version"),
				RawType.DateTime("VersionDate"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(4, result.Count);
			Assert.AreEqual(1, (byte)result["SystemInformationID"]);
			Assert.AreEqual("10.50.91013.00", result["Database Version"]);
			Assert.AreEqual(new DateTime(2009, 10, 13), (DateTime)result["VersionDate"]);
			Assert.AreEqual(new DateTime(2009, 10, 13), (DateTime)result["ModifiedDate"]);
		}

		[Test]
		public void Parse_DatabaseLog()
		{
			var db = new RawDatabase(AWPath);
			var page = db.GetPage(1, 150);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.Int("DatabaseLogID"),
				RawType.DateTime("PostTime"),
				RawType.NVarchar("DatabaseUser"),
				RawType.NVarchar("Event"),
				RawType.NVarchar("Schema"),
				RawType.NVarchar("Object"),
				RawType.NVarchar("TSQL"),
				RawType.Xml("XmlEvent")
			});

			Assert.AreEqual(8, result.Count);
			Assert.AreEqual(1, (int)result["DatabaseLogID"]);
			Assert.AreEqual(new DateTime(2012, 03, 29, 13, 52, 01, 163), (DateTime)result["PostTime"]);
			Assert.AreEqual("dbo", result["DatabaseUser"]);
			Assert.AreEqual("CREATE_TABLE", result["Event"]);
			Assert.AreEqual("dbo", result["Schema"]);
			Assert.AreEqual("ErrorLog", result["Object"]);
			Assert.AreEqual(451, result["TSQL"].ToString().Length);
			Assert.AreEqual(1745, ((byte[])result["XmlEvent"]).Length);
		}

		[Test]
		public void Parse_HumanResources_Department()
		{
			var db = new RawDatabase(AWPath);
			var page = db.GetPage(1, 665);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.SmallInt("DepartmentID"),
				RawType.NVarchar("Name"),
				RawType.NVarchar("GroupName"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(4, result.Count);
			Assert.AreEqual(1, (short)result["DepartmentID"]);
			Assert.AreEqual("Engineering", result["Name"]);
			Assert.AreEqual("Research and Development", result["GroupName"]);
			Assert.AreEqual(new DateTime(2002, 06, 01), (DateTime)result["ModifiedDate"]);
		}

		[Test]
		public void Parse_HumanResources_Employee()
		{
			var db = new RawDatabase(AWPath);
			var page = db.GetPage(1, 3792);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.Int("BusinessEntityID"),
				RawType.NVarchar("NationalIDNumber"),
				RawType.NVarchar("LoginID"),
				RawType.HierarchyID("OrganizationNode"),
				RawType.NVarchar("JobTitle"),
				RawType.Date("BirthDate"),
				RawType.NChar("MaritalStatus", 1),
				RawType.NChar("Gender", 1),
				RawType.Date("HireDate"),
				RawType.Bit("SalariedFlag"),
				RawType.SmallInt("VacationHours"),
				RawType.SmallInt("SickLeaveHours"),
				RawType.Bit("CurrentFlag"),
				RawType.UniqueIdentifier("rowguid"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(15, result.Count);
			Assert.AreEqual(1, (int)result["BusinessEntityID"]);
			Assert.AreEqual("295847284", result["NationalIDNumber"].ToString());
			Assert.AreEqual(@"adventure-works\ken0", result["LoginID"].ToString());
			Assert.AreEqual(0, ((byte[])result["OrganizationNode"]).Length);
			Assert.AreEqual("Chief Executive Officer", result["JobTitle"].ToString());
			Assert.AreEqual(new DateTime(1963, 03, 02), (DateTime)result["BirthDate"]);
			Assert.AreEqual("S", result["MaritalStatus"].ToString());
			Assert.AreEqual("M", result["Gender"].ToString());
			Assert.AreEqual(new DateTime(2003, 02, 15), (DateTime)result["HireDate"]);
			Assert.AreEqual(true, (bool)result["SalariedFlag"]);
			Assert.AreEqual(99, (short)result["VacationHours"]);
			Assert.AreEqual(69, (short)result["SickLeaveHours"]);
			Assert.AreEqual(true, (bool)result["CurrentFlag"]);
			Assert.AreEqual(new Guid("f01251e5-96a3-448d-981e-0f99d789110d"), (Guid)result["rowguid"]);
			Assert.AreEqual(new DateTime(2008, 7, 31), (DateTime)result["ModifiedDate"]);
		}
	}
}