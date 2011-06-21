using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class Heaptest : DataRow
	{
		public Heaptest()
		{
			Columns.Add(new DataColumn("ID", "int"));
			Columns.Add(new DataColumn("Filler", "varchar(8000)", true));
			Columns.Add(new DataColumn("Name", "varchar(max)", true));
		}
	}
}