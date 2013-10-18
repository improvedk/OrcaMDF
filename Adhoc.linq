<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference Relative="src\OrcaMDF.RawCore\bin\Debug\OrcaMDF.Core.dll">E:\Projects\OrcaMDF (GIT)\src\OrcaMDF.RawCore\bin\Debug\OrcaMDF.Core.dll</Reference>
  <Reference Relative="src\OrcaMDF.RawCore\bin\Debug\OrcaMDF.RawCore.dll">E:\Projects\OrcaMDF (GIT)\src\OrcaMDF.RawCore\bin\Debug\OrcaMDF.RawCore.dll</Reference>
  <Namespace>OrcaMDF.Core.Engine.Records</Namespace>
  <Namespace>OrcaMDF.RawCore</Namespace>
  <Namespace>OrcaMDF.Core.Engine.Pages</Namespace>
</Query>

// Get single data page
var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.GetPage(1, 5000).Dump();

// Get single index page
var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.GetPage(1, 77).Dump();

// Get all distinct types of pages
var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.Pages
	.Select(x => x.Header.Type)
	.Distinct()
	.Dump();
	
// Get all pages of a certain type
var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.Pages
	.Where(x => x.Header.Type == PageType.IAM)
	.Dump();
	
// Get all index pages with their slot count
var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.Pages
	.Where(x => x.Header.Type == PageType.Index)
	.Select(x => new {
		x.PageID,
		x.Header.SlotCnt
	}).Dump();

// Get certain properties of all pages
var db = new RawDatabase(@"C:\AW2008R2.mdf"); 	
db.Pages.Select(x => new {
	x.PageID,
	x.Header.Type,
	x.Header.SlotCnt
}).Dump();

// Predicate search for all pages
var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.Pages
	.Where(x => x.Header.FreeData > 7000)
	.Where(x => x.Header.SlotCnt >= 1)
	.Select(x => new {
	    x.PageID,
		x.Header.FreeData,
		RecordCount = x.Records.Count()
	}).Dump();