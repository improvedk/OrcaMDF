using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class Heaptest
	{
		[Column("int", 1)]
		public int ID { get; set; }

		[Column("varchar(8000)", 2)]
		public string Filler { get; set; }

		[Column("varchar(50)", 3)]
		public string Name { get; set; }
	}
}