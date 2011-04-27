using System;

namespace OrcaMDF.Core.Engine
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ColumnAttribute : Attribute
	{
		public ColumnType Type { get; private set; }
		public string TypeString { get; private set; }
		public short? Length { get; private set; }
		public bool Nullable { get; set; }

		public ColumnAttribute(string type)
		{
			var typeDesc = ColumnTypeDescriptionFactory.GetDescription(type);
			Type = typeDesc.Type;
			Length = typeDesc.VariableFixedLength;
			TypeString = type;
		}
	}
}