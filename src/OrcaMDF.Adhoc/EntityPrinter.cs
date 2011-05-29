using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.Adhoc
{
	class EntityPrinter
	{
		public static void Print<T>(IEnumerable<T> input)
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

			foreach (var prop in typeof(T).GetProperties())
			{
				int maxPropValueLength = entities.Max(x => Math.Max((prop.GetValue(x, null) ?? "").ToString().Length, 6));

				maxPropValueLength = Math.Min(maxPropValueLength, 40);

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
					{
						if (value.ToString().Length > propLengths[prop.Name])
							Console.Write(("<" + value.ToString().Length.ToString().PadLeft(5, '0') + " chars>").PadRight(propLengths[prop.Name]));
						else
							Console.Write(value.ToString().PadRight(propLengths[prop.Name]));
					}
				}

				Console.WriteLine();
				cnt++;
			}
		}
	}
}