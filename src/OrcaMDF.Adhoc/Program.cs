using System;
using System.Linq;
using OrcaMDF.Adhoc.Entities;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main()
        {
        	Console.WriteLine("VSS Copying...");
        	VssHelper.CopyFile(@"C:\MOW.mdf", @"C:\MOW_Copy.mdf");
        	Console.WriteLine();

			using (var file = new MdfFile(@"C:\MOW_Copy.mdf"))
			{
				var scanner = new DataScanner(file);
				var slobs = scanner.ScanTable<HeapPerson>("HeapPersons").Take(100);
				EntityPrinter.Print(slobs);
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}