using System.Collections.Generic;

namespace OrcaMDF.Core.MetaData.ObjectDefinitions
{
	public class TableDefinition
	{
		public string Name { get; set; }
		public IList<ColumnDefinition> Columns { get; private set; }
		public IList<IndexDefinition> Indexes { get; private set; }
	}
}