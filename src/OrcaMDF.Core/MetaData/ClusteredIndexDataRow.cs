using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData
{
	public class ClusteredTableIndexRow : DataRow
	{
		public PagePointer PagePointer { get; internal set; }

		// TODO
		public ClusteredTableIndexRow() : base(new DataColumn[0])
		{ }
	}
}