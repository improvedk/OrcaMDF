using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData
{
	public abstract class ClusteredIndexEntity
	{
		public PagePointer ChildPage { get; set; }
	}
}