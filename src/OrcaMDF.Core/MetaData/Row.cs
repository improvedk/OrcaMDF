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

		private HashSet<string> columnNames;

		protected Row()
			: this(new List<DataColumn>())
		{ }

		protected Row(IList<DataColumn> columns)
		{
			Columns = columns;
			data = new Dictionary<string, object>();
		}

		private void ensureColumnExists(string name)
		{
			if(columnNames == null)
			{
				columnNames = new HashSet<string>();

				foreach(var col in Columns)
					columnNames.Add(col.Name);
			}

			if(!columnNames.Contains(name))
				throw new ArgumentOutOfRangeException("Column '" + name + "' does not exist.");
		}

		public T Field<T>(DataColumn col)
		{
			return Field<T>(col.Name);
		}

		public T Field<T>(string name)
		{
			ensureColumnExists(name);

			// We need to handle nullables explicitly
			Type t = typeof (T);
			Type u = Nullable.GetUnderlyingType(t);
			
			if(u != null)
			{
				if (!data.ContainsKey(name) || data[name] == null)
					return default(T);

				return (T)Convert.ChangeType(data[name], u);
			}

			object value = data.ContainsKey(name) ? data[name] : null;
			return (T)Convert.ChangeType(value, t);
		}

		public object this[string name]
		{
			get
			{
				ensureColumnExists(name);

				if (data.ContainsKey(name))
					return data[name];

				return null;
			}
			set
			{
				ensureColumnExists(name);

				data[name] = value;
			}
		}

		public object this[DataColumn col]
		{
			get { return this[col.Name]; }
			set { this[col.Name] = value; }
		}

		public abstract Row NewRow();
	}
}