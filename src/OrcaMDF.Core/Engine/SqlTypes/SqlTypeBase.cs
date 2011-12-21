namespace OrcaMDF.Core.Engine.SqlTypes
{
	public abstract class SqlTypeBase : ISqlType
	{
		protected CompressionContext CompressionContext;

		protected SqlTypeBase(CompressionContext compression)
		{
			CompressionContext = compression;
		}

		public abstract bool IsVariableLength { get; }
		public abstract short? FixedLength { get; }
		public abstract object GetValue(byte[] value);
	}
}