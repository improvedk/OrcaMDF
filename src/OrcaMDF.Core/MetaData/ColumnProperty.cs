using System.Reflection;

namespace OrcaMDF.Core.MetaData
{
	public class ColumnProperty
	{
		public ColumnAttribute Column { get; private set; }
		public PropertyInfo Property { get; private set; }

		public ColumnProperty(ColumnAttribute column, PropertyInfo property)
		{
			Column = column;
			Property = property;
		}
	}
}