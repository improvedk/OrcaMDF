using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Engine.Records.VariableLengthDataProxies
{
	public class DataProxy
	{
		protected Page OriginPage;

		protected DataProxy(Page page)
		{
			OriginPage = page;
		}
	}
}