namespace OrcaMDF.Core.Engine
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

		public override string ToString()
		{
			if (VariableFixedLength != null)
				return Type.ToString().ToLower() + "(" + VariableFixedLength + ")";
			
			return Type.ToString().ToLower();
		}
	}
}