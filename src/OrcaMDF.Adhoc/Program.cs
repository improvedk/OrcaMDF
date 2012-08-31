using System;
using System.IO;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main()
        {
        	string mdfPath = @"D:\MSSQL Databases\Corrupt1.mdf";

			using (var fs = File.Open(mdfPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				var headerBytes = new byte[96];
				
			}

        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}