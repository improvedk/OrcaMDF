using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.Adhoc
{
	class EntityPrinter
	{
		public static void Print<T>(IEnumerable<T> entities)
		{
			var propLengths = new Dictionary<string, int>();
			
			// Add row number column
			propLengths.Add("#", 8);
			Console.Write("#".PadRight(propLengths["#"]));

			foreach (var prop in typeof(T).GetProperties())
			{
				int maxPropValueLength = entities.Max(x => Math.Max((prop.GetValue(x, null) ?? "").ToString().Length, 6));

				if (prop.Name.Length > maxPropValueLength)
					maxPropValueLength = prop.Name.Length;

				propLengths.Add(prop.Name, maxPropValueLength + 2);

				Console.Write(prop.Name.PadRight(maxPropValueLength + 2));
			}

			Console.WriteLine();

			int cnt = 1;
			foreach (var entity in entities)
			{
				Console.Write(cnt.ToString().PadRight(propLengths["#"]));

				foreach (var prop in typeof(T).GetProperties())
				{
					object value = prop.GetValue(entity, null);

					if (value == null)
						Console.Write("<null>".PadRight(propLengths[prop.Name]));
					else
						Console.Write(prop.GetValue(entity, null).ToString().PadRight(propLengths[prop.Name]));
				}

				Console.WriteLine();
				cnt++;
			}
		}
	}
}