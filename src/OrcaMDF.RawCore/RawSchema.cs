using System.Collections.Generic;

namespace OrcaMDF.RawCore
{
	public class RawSchema
	{
		/*
		public static string[] ParseFixedLengthData(IEnumerable<byte> bytes, IEnumerable<ISqlType> schema)
		{
			var dates = bytes.ToArray();

			var result = new List<string>();
			short index = 0;
			foreach (var type in schema.Where(x => !x.IsVariableLength))
			{
				var typeBytes = dates.Skip(index).Take(type.FixedLength.Value).ToArray();
				var value = type.GetValue(typeBytes);
				index += (short)typeBytes.Length;

				result.Add(value.ToString());
			}

			return result.ToArray();
		}
		*/
	}
}