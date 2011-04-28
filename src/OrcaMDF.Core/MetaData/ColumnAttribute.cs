using System;

namespace OrcaMDF.Core.MetaData
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnAttribute : Attribute
	{
		public ColumnTypeDescription Description { get; private set; }
		public bool Nullable { get; set; }

		public ColumnAttribute(string type)
		{
			Description = new ColumnTypeDescription(type);
		}
	}
}