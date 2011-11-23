using System.Collections.ObjectModel;

namespace OrcaMDF.Core.MetaData
{
	public interface ISchema
	{
		ReadOnlyCollection<DataColumn> Columns { get; }
		bool HasColumn(string name);
	}
}