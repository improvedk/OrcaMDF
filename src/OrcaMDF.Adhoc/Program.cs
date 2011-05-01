using System;
using System.Diagnostics;
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
				var sw = new Stopwatch();
				sw.Start();
				var slobs = scanner.ScanClusteredIndexUsingIndexStructure<PersonCIndex, Person>(new PagePointer(1, 126)).Where(x => x.Name.StartsWith("M")).Take(10);
				EntityPrinter.Print(slobs);
				Console.WriteLine(sw.ElapsedTicks);
				sw.Reset();
				sw.Start();
				scanner.ScanClusteredIndexUsingIndexStructure<PersonCIndex, Person>(new PagePointer(1, 126)).Take(10000);
				Console.WriteLine(sw.ElapsedTicks);

				//EntityPrinter.Print(slobs);

				/*var metaData = file.GetMetaData();
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
					Console.WriteLine(table);*/
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}