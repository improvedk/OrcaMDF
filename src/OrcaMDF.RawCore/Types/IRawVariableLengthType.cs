namespace OrcaMDF.RawCore.Types
{
	public interface IRawVariableLengthType : IRawType
	{
		object EmptyValue { get; }
	}
}