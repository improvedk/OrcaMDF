using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class HeapPerson
	{
		[Column("int", 1)]
		public int ID { get; set; }

		[Column("int", 2)]
		public int Age { get; set; }

		[Column("nvarchar", 3)]
		public string Name { get; set; }

		[Column("nvarchar", 4)]
		public string Email { get; set; }
	}
}