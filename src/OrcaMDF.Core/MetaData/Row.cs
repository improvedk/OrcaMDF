using System;
using System.Collections.Generic;

namespace OrcaMDF.Core.MetaData
{
	/// <summary>
	/// Most inefficient to store schema with each and every row. Simplicity wins over efficiency for now. Can be optimized later.
	/// </summary>
	public abstract class Row
	{
		public IList<DataColumn> Columns { get; private set; }

		protected IDictionary<string, object> data;

		protected Row()
			: this(new List<DataColumn>())
		{ }

		protected Row(IList<DataColumn> columns)
		{
			Columns = columns;
			data = new Dictionary<string, object>();
		}

		public T Field<T>(string name)
		{
			return (T)Convert.ChangeType(data[name], typeof(T));
		}

		public object this[string name]
		{
			get { return data[name]; }
			set { data[name] = value; }
		}

		public object this[DataColumn col]
		{
			get { return data[col.Name]; }
			set { data[col.Name] = value; }
		}

		public abstract Row NewRow();
	}
}