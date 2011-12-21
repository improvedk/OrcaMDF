namespace OrcaMDF.Core.Engine
{
	public class CompressionContext
	{
		internal CompressionLevel CompressionLevel { get; private set; }
		internal bool UsesVardecimals { get; private set; }

		internal CompressionContext(CompressionLevel compressionLevel, bool usesVardecimals)
		{
			CompressionLevel = compressionLevel;
			UsesVardecimals = usesVardecimals;
		}

		internal static CompressionContext NoCompression
		{
			get { return new CompressionContext(CompressionLevel.None, false); }
		}
	}
}