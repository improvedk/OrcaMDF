using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.Core.MetaData
{
	public class DataRow : Row
	{
		public DataRow(IEnumerable<DataColumn> columns)
			: base(new Schema(columns))
		{ }

		public DataRow(ISchema schema) : base(schema)
		{ }

		public override Row NewRow()
		{
			return new DataRow(Schema);
		}

		public bool HasSparseColumns
		{
			get { return Columns.Any(x => x.IsSparse); }
		}
	}
}