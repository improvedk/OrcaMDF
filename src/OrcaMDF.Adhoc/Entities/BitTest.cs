using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class BitTest
	{
		[Column("int", 1)]
		public int ID { get; set; }

		[Column("bit", 2)]
		public bool A { get; set; }

		[Column("bit", 3)]
		public bool B { get; set; }

		[Column("bit", 4)]
		public bool C { get; set; }

		[Column("bit", 5)]
		public bool D { get; set; }

		[Column("bit", 6)]
		public bool E { get; set; }

		[Column("bit", 7)]
		public bool F { get; set; }

		[Column("bit", 8)]
		public bool G { get; set; }

		[Column("char(4)", 9)]
		public string PostCode { get; set; }

		[Column("bit", 10)]
		public bool H { get; set; }

		[Column("bit", 11)]
		public bool I { get; set; }
	}
}