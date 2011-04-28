using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class BitTest
	{
		[Column("int")]
		public int ID { get; set; }

		[Column("bit")]
		public bool A { get; set; }

		[Column("bit")]
		public bool B { get; set; }

		[Column("bit")]
		public bool C { get; set; }

		[Column("bit")]
		public bool D { get; set; }

		[Column("bit")]
		public bool E { get; set; }

		[Column("bit")]
		public bool F { get; set; }

		[Column("bit")]
		public bool G { get; set; }

		[Column("char(4)")]
		public string PostCode { get; set; }

		[Column("bit")]
		public bool H { get; set; }

		[Column("bit")]
		public bool I { get; set; }
	}
}