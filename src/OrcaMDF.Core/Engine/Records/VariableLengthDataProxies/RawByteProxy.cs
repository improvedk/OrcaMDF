using System.Collections.Generic;

namespace OrcaMDF.Core.Engine.Records.VariableLengthDataProxies
{
	public class RawByteProxy : IVariableLengthDataProxy
	{
		private readonly byte[] data;

		public RawByteProxy(byte[] data)
		{
			this.data = data;
		}

		public IEnumerable<byte> GetBytes()
		{
			return data;
		}
	}
}