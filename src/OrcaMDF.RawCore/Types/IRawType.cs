namespace OrcaMDF.RawCore.Types
{
	public interface IRawFixedLengthType
	{
		short Length { get; }
		object GetValue(byte[] bytes);
	}
}