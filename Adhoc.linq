<Query Kind="Statements">
  <Reference Relative="src\OrcaMDF.RawCore\bin\Debug\OrcaMDF.RawCore.dll">E:\Projects\OrcaMDF (GIT)\src\OrcaMDF.RawCore\bin\Debug\OrcaMDF.RawCore.dll</Reference>
  <Namespace>OrcaMDF.RawCore</Namespace>
</Query>

var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.GetPage(1, 50).Dump();


var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.GetAllPages(1).Select(x => new {
	x.PageID,
	x.Header.Type,
	x.Header.SlotCnt
}).Dump();


var db = new RawDatabase(@"C:\AW2008R2.mdf");
db.GetAllPages(1)
	.Where(x => x.Header.FreeData > 7000)
	.Where(x => x.Header.SlotCnt >= 1)
	.Select(x => new {
	    x.PageID,
		x.Header.FreeData
	}).Dump();