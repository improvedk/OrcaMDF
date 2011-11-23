using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OrcaMDF.Core.MetaData
{
	public class Schema : ISchema
	{
		private readonly List<DataColumn> columns = new List<DataColumn>();
		private readonly HashSet<string> columnNameCache = new HashSet<string>();
		
		public Schema(IEnumerable<DataColumn> columns)
		{
			this.columns.AddRange(columns);

			foreach(var col in columns)
				columnNameCache.Add(col.Name);
		}

		public ReadOnlyCollection<DataColumn> Columns
		{
			get { return columns.AsReadOnly(); }
		}

		public bool HasColumn(string name)
		{
			return columnNameCache.Contains(name);
		}
	}
}