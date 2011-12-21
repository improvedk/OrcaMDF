namespace OrcaMDF.Core.Engine.SqlTypes
{
	public interface ISqlType
	{
		bool IsVariableLength { get; }
		short? FixedLength { get; }
		object GetValue(byte[] value);
	}
}