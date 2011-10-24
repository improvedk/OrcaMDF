using System;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main()
        {
        	//Console.WriteLine("VSS Copying...");
			//VssHelper.CopyFile(@"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\Data1.mdf", @"C:\Data1.mdf");
        	//Console.WriteLine();

        	var files = new[]
        	    {
					@"C:\SampleDatabase_Data1.mdf",
					@"C:\SampleDatabase_Data2.ndf",
					@"C:\SampleDatabase_Data3.ndf"
        	    };

			using (var db = new Database(files))
			{
				var scanner = new DataScanner(db);
				var result = scanner.ScanTable("MyTable");

				EntityPrinter.Print(result);
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}