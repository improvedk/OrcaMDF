using System.Linq;
using System;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main()
        {
			using (var db = new Database(new[] { @"D:\Test.mdf" }))
			{
				// The table we're interested in
				var table = db.Dmvs.Tables.Single(x => x.Name == "Test");

				// Get all rows
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable(table.Name);
				
				EntityPrinter.Print(rows);
			}
			
        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}