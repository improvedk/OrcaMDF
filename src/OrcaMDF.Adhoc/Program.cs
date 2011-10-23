using System;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main()
        {
        	Console.WriteLine("VSS Copying...");
			//VssHelper.CopyFile(@"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Data1.mdf", @"C:\Data1.mdf");
        	Console.WriteLine();

			using (var db = new Database(new[] { @"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MF1.mdf", @"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MF2.ndf" }))
			{
				var scanner = new DataScanner(db);

				var result = scanner.ScanTable("X");
				EntityPrinter.Print(result);
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}