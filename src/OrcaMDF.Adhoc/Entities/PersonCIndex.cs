using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class PersonCIndex : ClusteredIndexEntity
	{
		[Column("int", 1)]
		public int ID { get; set; }

		[Column("int", 2, Nullable = true)]
		public int Age { get; set; }
	}
}