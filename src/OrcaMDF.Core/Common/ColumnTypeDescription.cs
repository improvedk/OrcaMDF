using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Common
{
	public class ColumnTypeDescription
	{
		public readonly ColumnType Type;
		public readonly short? VariableFixedLength;

		public ColumnTypeDescription(ColumnType type, short? variableFixedLength)
		{
			Type = type;
			VariableFixedLength = variableFixedLength;
		}
	}
}