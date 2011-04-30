using System;
using OrcaMDF.Core;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main(string[] args)
        {
        	Console.WriteLine("VSS Copying...");
			//VssHelper.CopyFile(@"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Test.mdf", @"C:\MOW_Copy.mdf");
        	VssHelper.CopyFile(@"C:\MOW.mdf", @"C:\MOW_Copy.mdf");
        	Console.WriteLine("Done");
        	Console.WriteLine();

			using (var file = new MdfFile(@"C:\MOW_Copy.mdf"))
			{
				// Data page
				//var dataPage = file.GetDataPage(22);
				//var slobs = dataPage.GetEntities<Person>();
				//Console.WriteLine(dataPage);
				//EntityPrinter.Print(slobs);

				// BitTest
				//var dataPage = file.GetDataPage(94);
				//var slobs = dataPage.GetEntities<BitTest>();
				//EntityPrinter.Print(slobs);

				//var dataPage2 = file.GetTextMixPage(79);
				//var slobs = dataPage.GetEntities<Person>();
				//EntityPrinter.Print(slobs);

				// IAM page
				//var iamPage = file.GetIamPage(55);
				//Console.WriteLine(iamPage);

				// GAM page
				//var gamPage = file.GetGamPage(2);
				//Console.WriteLine(gamPage);

				// SGAM page
				//var sgamPage = file.GetSgamPage(3);
				//Console.WriteLine(sgamPage);

				// PFS page
				//var pfsPage = file.GetPfsPage(1);
				//Console.WriteLine(pfsPage);

				// Boot page
				//var bootPage = file.GetBootPage(9);
				//Console.WriteLine(bootPage);

				//var allocUnitPage = file.GetDataPage(16);
				//var allocUnits = allocUnitPage.GetEntities<SysAllocationUnit>();
				//EntityPrinter.Print(allocUnits);

				//var scanner = new DataScanner(file);
				//var slobs = scanner.ScanLinkedPages<SysObject>(new PageLocation(1, 116));
				//EntityPrinter.Print(slobs);

				var metaData = file.GetMetaData();
				Console.WriteLine(metaData.SysAllocationUnits.Count);
				Console.WriteLine(metaData.SysRowsets.Count);
				Console.WriteLine(metaData.SysRowsetColumns.Count);
				Console.WriteLine(metaData.SysObjects.Count);
				Console.WriteLine();

				Console.WriteLine("User tables:");
				foreach(string table in metaData.UserTableNames)
					Console.WriteLine(table);

				Console.WriteLine();

				Console.WriteLine("System tables:");
				foreach (string table in metaData.TableNames)
					Console.WriteLine(table);
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}