using System;
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
				file.GetGamPage(new PagePointer(1, 2));
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}