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
			Assert.AreEqual(1, result["SystemInformationID"]);
			Assert.AreEqual(databaseVersion, result["Database Version"]);
			Assert.AreEqual(Convert.ToDateTime(versionDate), result["VersionDate"]);
			Assert.AreEqual(Convert.ToDateTime(versionDate), result["ModifiedDate"]);
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
			Assert.AreEqual(9, result["AddressID"]);
			Assert.AreEqual("8713 Yosemite Ct.", result["AddressLine1"]);
			Assert.AreEqual(null, result["AddressLine2"]);
			Assert.AreEqual("Bothell", result["City"]);
			Assert.AreEqual("Washington", result["StateProvince"]);
			Assert.AreEqual("United States", result["CountryRegion"]);
			Assert.AreEqual("98011", result["PostalCode"]);
			Assert.AreEqual(new Guid("268af621-76d7-4c78-9441-144fd139821a"), result["rowguid"]);
			Assert.AreEqual(new DateTime(2002, 07, 01), result["ModifiedDate"]);
		}

		[TestCase(AW2005Path, 408, "2004-10-13 11:15:07.263", TestName = "2005")]
		[TestCase(AW2008Path, 448, "2001-08-01", TestName = "2008")]
		[TestCase(AW2008R2Path, 546, "2001-08-01", TestName = "2008R2")]
		[TestCase(AW2012Path, 448, "2001-08-01", TestName = "2012")]
		public void Parse_Customer(string dbPath, int pageID, string modifiedDate)
		{
			var db = new RawDatabase(dbPath);
			var page = db.GetPage(1, pageID);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.Int("CustomerID"),
				RawType.Bit("NameStyle"),
				RawType.NVarchar("Title"),
				RawType.NVarchar("FirstName"),
				RawType.NVarchar("MiddleName"),
				RawType.NVarchar("LastName"),
				RawType.NVarchar("Suffix"),
				RawType.NVarchar("CompanyName"),
				RawType.NVarchar("SalesPerson"),
				RawType.NVarchar("EmailAddress"),
				RawType.NVarchar("Phone"),
				RawType.Varchar("PasswordHash"),
				RawType.Varchar("PasswordSalt"),
				RawType.UniqueIdentifier("rowguid"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(15, result.Count);
			Assert.AreEqual(1, result["CustomerID"]);
			Assert.AreEqual(false, result["NameStyle"]);
			Assert.AreEqual("Mr.", result["Title"]);
			Assert.AreEqual("Orlando", result["FirstName"]);
			Assert.AreEqual("N.", result["MiddleName"]);
			Assert.AreEqual("Gee", result["LastName"]);
			Assert.AreEqual(null, result["Suffix"]);
			Assert.AreEqual("A Bike Store", result["CompanyName"]);
			Assert.AreEqual(@"adventure-works\pamela0", result["SalesPerson"]);
			Assert.AreEqual("orlando0@adventure-works.com", result["EmailAddress"]);
			Assert.AreEqual("245-555-0173", result["Phone"]);
			Assert.AreEqual("L/Rlwxzp4w7RWmEgXX+/A7cXaePEPcp+KwQhl2fJL7w=", result["PasswordHash"]);
			Assert.AreEqual("1KjXYs4=", result["PasswordSalt"]);
			Assert.AreEqual(new Guid("3f5ae95e-b87d-4aed-95b4-c3797afcb74f"), result["rowguid"]);
			Assert.AreEqual(Convert.ToDateTime(modifiedDate), result["ModifiedDate"]);
		}

		[TestCase(AW2005Path, 90, 1, 832, "314f2574-1f75-457f-9bd1-74d1ce53daa5", "2001-08-01", TestName = "2005")]
		[TestCase(AW2008Path, 178, 29485, 1086, "16765338-dbe4-4421-b5e9-3836b9278e63", "2003-09-01", TestName = "2008")]
		[TestCase(AW2008R2Path, 109, 29485, 1086, "16765338-dbe4-4421-b5e9-3836b9278e63", "2003-09-01", TestName = "2008R2")]
		[TestCase(AW2012Path, 178, 29485, 1086, "16765338-dbe4-4421-b5e9-3836b9278e63", "2003-09-01", TestName = "2012")]
		public void Parse_CustomerAddress(string dbPath, int pageID, int customerID, int addressID, string rowguid, string modifiedDate)
		{
			var db = new RawDatabase(dbPath);
			var page = db.GetPage(1, pageID);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.Int("CustomerID"),
				RawType.Int("AddressID"),
				RawType.NVarchar("AddressType"),
				RawType.UniqueIdentifier("rowguid"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(5, result.Count);
			Assert.AreEqual(customerID, result["CustomerID"]);
			Assert.AreEqual(addressID, result["AddressID"]);
			Assert.AreEqual("Main Office", result["AddressType"]);
			Assert.AreEqual(new Guid(rowguid), result["rowguid"]);
			Assert.AreEqual(Convert.ToDateTime(modifiedDate), result["ModifiedDate"]);
		}

		[TestCase(AW2005Path, 448, TestName = "2005")]
		[TestCase(AW2008Path, 520, TestName = "2008")]
		[TestCase(AW2008R2Path, 517, TestName = "2008R2")]
		[TestCase(AW2012Path, 520, TestName = "2012")]
		public void Parse_Product(string dbPath, int pageID)
		{
			var db = new RawDatabase(dbPath);
			var page = db.GetPage(1, pageID);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.Int("ProductID"),
				RawType.NVarchar("Name"),
				RawType.NVarchar("ProductNumber"),
				RawType.NVarchar("Color"),
				RawType.Money("StandardCost"),
				RawType.Money("ListPrice"),
				RawType.NVarchar("Size"),
				RawType.Decimal("Weight", 8, 2),
				RawType.Int("ProductCategoryID"),
				RawType.Int("ProductModelID"),
				RawType.DateTime("SellStartDate"),
				RawType.DateTime("SellEndDate"),
				RawType.DateTime("DiscontinuedDate"),
				RawType.VarBinary("ThumbNailPhoto"),
				RawType.NVarchar("ThumbnailPhotoFileName"),
				RawType.UniqueIdentifier("rowguid"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(17, result.Count);
			Assert.AreEqual(680, result["ProductID"]);
			Assert.AreEqual("HL Road Frame - Black, 58", result["Name"]);
			Assert.AreEqual("FR-R92B-58", result["ProductNumber"]);
			Assert.AreEqual("Black", result["Color"]);
			Assert.AreEqual(1059.31, result["StandardCost"]);
			Assert.AreEqual(1431.50, result["ListPrice"]);
			Assert.AreEqual("58", result["Size"]);
			Assert.AreEqual(1016.04, result["Weight"]);
			Assert.AreEqual(18, result["ProductCategoryID"]);
			Assert.AreEqual(6, result["ProductModelID"]);
			Assert.AreEqual(Convert.ToDateTime("1998-06-01"), result["SellStartDate"]);
			Assert.AreEqual(null, result["SellEndDate"]);
			Assert.AreEqual(null, result["DiscontinuedDate"]);
			Assert.AreEqual(1077, ((byte[])result["ThumbNailPhoto"]).Length);
			Assert.AreEqual("no_image_available_small.gif", result["ThumbnailPhotoFileName"]);
			Assert.AreEqual(new Guid("43dd68d6-14a4-461f-9069-55309d90ea7e"), result["rowguid"]);
			Assert.AreEqual(Convert.ToDateTime("2004-03-11 10:01:36.827"), result["ModifiedDate"]);
		}

		[TestCase(AW2005Path, 93, TestName = "2005")]
		[TestCase(AW2008Path, 186, TestName = "2008")]
		[TestCase(AW2008R2Path, 212, TestName = "2008R2")]
		[TestCase(AW2012Path, 186, TestName = "2012")]
		public void Parse_ProductCategory(string dbPath, int pageID)
		{
			var db = new RawDatabase(dbPath);
			var page = db.GetPage(1, pageID);
			var record = page.Records.First() as RawPrimaryRecord;

			var result = RawColumnParser.Parse(record, new IRawType[] {
				RawType.Int("ProductCategoryID"),
				RawType.Int("ParentProductCategoryID"),
				RawType.NVarchar("Name"),
				RawType.UniqueIdentifier("rowguid"),
				RawType.DateTime("ModifiedDate")
			});

			Assert.AreEqual(5, result.Count);
			Assert.AreEqual(1, result["ProductCategoryID"]);
			Assert.AreEqual(null, result["ParentProductCategoryID"]);
			Assert.AreEqual("Bikes", result["Name"]);
			Assert.AreEqual(new Guid("cfbda25c-df71-47a7-b81b-64ee161aa37c"), result["rowguid"]);
			Assert.AreEqual(Convert.ToDateTime("1998-06-01"), result["ModifiedDate"]);
		}
	}
}