using System;
using OrcaMDF.Adhoc.Entities;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;

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
				var rows = scanner.ScanTable<Heaptest>("Heaptest");
				EntityPrinter.Print(rows);
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}