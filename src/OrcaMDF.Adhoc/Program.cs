using System;
using OrcaMDF.Core;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData.SystemEntities;

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
				//var allocUnits = allocUnitPage.GetEntities<AllocationUnit>();
				//EntityPrinter.Print(allocUnits);

				var scanner = new DataScanner(file);
				var slobs = scanner.ScanLinkedPages<Rowset>(new PageLocation(1, 18));
				EntityPrinter.Print(slobs);
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}