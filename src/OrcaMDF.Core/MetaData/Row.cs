using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OrcaMDF.Core.MetaData
{
	/// <summary>
	/// Stores the actual data contained in a row, including a reference to the row schema.
	/// </summary>
	public abstract class Row
	{
		protected ISchema Schema;
		
		protected IDictionary<string, object> data;

		public ReadOnlyCollection<DataColumn> Columns
		{
			get { return Schema.Columns; }
		}

		protected Row(ISchema schema)
		{
			Schema = schema;
			data = new Dictionary<string, object>();
		}

		private void ensureColumnExists(string name)
		{
			if(!Schema.HasColumn(name))
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

			// This is ugly, but fast as columns will practically always be present.
			// Exceptions are... The exception.
			try
			{
				return (T)Convert.ChangeType(data[name], t);
			}
			catch (KeyNotFoundException)
			{
				return (T)Convert.ChangeType(null, t);
			}
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