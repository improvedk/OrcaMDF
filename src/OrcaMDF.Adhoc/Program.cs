using System;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Adhoc
{
    class Program
    {
        static void Main()
        {
        	//Console.WriteLine("VSS Copying...");
			//VssHelper.CopyFile(@"C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\AdventureWorks_Data.mdf", @"C:\Test.mdf");
        	//Console.WriteLine();

			using (var db = new Database(new[] { @"C:\Test.mdf" }))
			{
				foreach (var ic in db.Dmvs.IndexColumns)
					Console.WriteLine(ic.ColumnID);

				Console.WriteLine(db.Dmvs.IndexColumns.Count());
			}
			
        	Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}