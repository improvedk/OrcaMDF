using System.Collections.Generic;

namespace OrcaMDF.Core.Engine.Records.VariableLengthDataProxies
{
	public interface IVariableLengthDataProxy
	{
		IEnumerable<byte> GetBytes();
	}
}