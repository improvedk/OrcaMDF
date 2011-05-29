using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class Heaptest
	{
		[Column("int", 1)]
		public int ID { get; set; }

		[Column("varchar(50)", 2)]
		public string Name { get; set; }
	}
}