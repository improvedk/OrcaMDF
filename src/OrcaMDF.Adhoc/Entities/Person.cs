using System;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class Person
	{
		[Column("int")]
		public int ID { get; set; }

		[Column("varchar")]
		public string Name { get; set; }

		[Column("char(1)", Nullable = true)]
		public string Sex { get; set; }

		[Column("int")]
		public int Age { get; set; }
		
		[Column("nvarchar")]
		public string Country { get; set; }
		
		[Column("datetime")]
		public DateTime Created { get; set; }
	}
}