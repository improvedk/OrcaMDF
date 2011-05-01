using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OrcaMDF.Core.MetaData
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnAttribute : Attribute
	{
		public ColumnTypeDescription Description { get; private set; }
		public short Ordinal { get; private set; }
		public bool Nullable { get; set; }

		public ColumnAttribute(string type, short ordinal)
		{
			Description = new ColumnTypeDescription(type);
			Ordinal = ordinal;
		}

		public static IList<ColumnProperty> GetOrderedColumnProperties<T>()
		{
			var result = new List<ColumnProperty>();

			foreach (PropertyInfo prop in typeof(T).GetProperties())
			{
				ColumnAttribute column = GetCustomAttribute(prop, typeof (ColumnAttribute)) as ColumnAttribute;

				if (column != null)
					result.Add(new ColumnProperty(column, prop));
			}

			// As the C# compiler does not guarantee 1-1 match between .cs declaration and compiled type, we need to sort by ordinal
			return result
				.OrderBy(x => x.Column.Ordinal)
				.ToList();
		}
	}
}