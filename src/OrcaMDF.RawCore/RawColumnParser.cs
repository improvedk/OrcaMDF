using OrcaMDF.RawCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.RawCore
{
	public class RawColumnParser
	{
		public static Dictionary<string, object> Parse(ArraySegment<byte> fixedLengthData, IEnumerable<ArraySegment<byte>> variableLengthData, IEnumerable<IRawType> schema)
		{
			return Parse(fixedLengthData.ToArray(), variableLengthData.Select(x => x.ToArray()).ToArray(), schema);
		}

		public static Dictionary<string, object> Parse(byte[] fixedLengthData, byte[][] variableLengthData, IEnumerable<IRawType> schema)
		{
			var result = new Dictionary<string, object>();
			int fixedIndex = 0;
			int variableIndex = 0;

			foreach (var type in schema)
			{
				object value;

				if (type is IRawFixedLengthType)
				{
					var fixedType = (IRawFixedLengthType)type;
					short fixedLength = fixedType.Length;

					value = fixedType.GetValue(fixedLengthData.Skip(fixedIndex).Take(fixedType.Length).ToArray());
					fixedIndex += fixedLength;
				}
				else
				{
					var variableType = (IRawVariableLengthType)type;
					value = variableType.GetValue(variableLengthData[variableIndex++]);
				}

				result.Add(type.Name, value);
			}

			return result;
		}
	}
}