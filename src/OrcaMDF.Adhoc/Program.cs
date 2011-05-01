using System;
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

			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}