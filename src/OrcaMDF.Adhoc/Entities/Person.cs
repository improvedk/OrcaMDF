using System;
using System.Data.Linq.Mapping;

namespace OrcaMDF.Adhoc.Entities
{
	public class Person
	{
		[Column(DbType = "int", CanBeNull = false)]
		public int ID { get; set; }

		[Column(DbType = "varchar", CanBeNull = false)]
		public string Name { get; set; }

		[Column(DbType = "char(1)",  CanBeNull = true)]
		public string Sex { get; set; }

		[Column(DbType = "int", CanBeNull = false)]
		public int Age { get; set; }
		
		[Column(DbType = "nvarchar", CanBeNull = false)]
		public string Country { get; set; }
		
		[Column(DbType = "datetime", CanBeNull = false)]
		public DateTime Created { get; set; }
	}
}