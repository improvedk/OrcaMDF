using System;
using System.Data;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main()
        {
        	Console.WriteLine("VSS Copying...");
			VssHelper.CopyFile(@"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Test.mdf", @"C:\Test.mdf");
        	Console.WriteLine();

			using (var file = new MdfFile(@"C:\Test.mdf"))
			{
				var scanner = new DataScanner(file);
				DataTable dt = scanner.ScanTable("Heaptest");
				EntityPrinter.Print(dt);

				DataTable result = new DataScanner(file).ScanTable("MyTable");
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}