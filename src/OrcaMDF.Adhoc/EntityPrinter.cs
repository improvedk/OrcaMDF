using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Adhoc
{
	class EntityPrinter
	{
		public static void Print<T>(IEnumerable<T> input) where T : DataRow, new()
		{
			var propLengths = new Dictionary<string, int>();
			var entities = input.ToList();

			if(entities.Count == 0)
			{
				Console.WriteLine("<Empty resultset>");
				return;
			}

			// Add row number column
			propLengths.Add("#", 8);
			Console.Write("#".PadRight(propLengths["#"]));

			// Get schema row
			var dr = new T();

			foreach (var col in dr.Columns)
			{
				int maxPropValueLength = entities.Max(x => Math.Max(x[col].ToString().Length, 6));

				maxPropValueLength = Math.Min(maxPropValueLength, 40);

				if (col.Name.Length > maxPropValueLength)
					maxPropValueLength = col.Name.Length;

				propLengths.Add(col.Name, maxPropValueLength + 2);

				Console.Write(col.Name.PadRight(maxPropValueLength + 2));
			}

			Console.WriteLine();

			int cnt = 1;
			foreach (var entity in entities)
			{
				Console.Write(cnt.ToString().PadRight(propLengths["#"]));

				foreach (var col in entity.Columns)
				{
					if (entity[col] == null)
						Console.Write("<null>".PadRight(propLengths[col.Name]));
					else
					{
						if (entity[col].ToString().Length > propLengths[col.Name])
							Console.Write(("<" + entity[col].ToString().Length.ToString().PadLeft(5, '0') + " chars>").PadRight(propLengths[col.Name]));
						else
							Console.Write(entity[col].ToString().PadRight(propLengths[col.Name]));
					}
				}

				Console.WriteLine();
				cnt++;
			}
		}
	}
}