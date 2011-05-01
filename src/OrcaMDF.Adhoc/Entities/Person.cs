using System;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc.Entities
{
	public class Person
	{
		[Column("int", 1)]
		public int ID { get; set; }

		[Column("varchar", 3, Nullable = true)]
		public string Name { get; set; }

		[Column("char(1)", 4, Nullable = true)]
		public string Sex { get; set; }

		[Column("int", 2, Nullable = true)]
		public int Age { get; set; }
		
		[Column("nvarchar", 5, Nullable = true)]
		public string Country { get; set; }
		
		[Column("datetime", 6, Nullable = true)]
		public DateTime Created { get; set; }
	}
}