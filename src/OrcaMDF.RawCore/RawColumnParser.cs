using OrcaMDF.Framework;
using OrcaMDF.RawCore.Records;
using OrcaMDF.RawCore.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.RawCore
{
	public class RawColumnParser
	{
		/// <summary>
		/// Tries to parse each page according to the schema while silently ignoring failures. Effectively
		/// this will try to brute-force parse each record on the page and return just the ones that
		/// physically match the schema.
		/// </summary>
		public static IEnumerable<dynamic> BestEffortParse(IEnumerable<RawPage> pages, IRawType[] schema)
		{
			foreach (var page in pages)
			{
				foreach (var record in page.BestEffortRecords)
				{
					dynamic parsedRecord = null;

					try
					{
						parsedRecord = Parse(record, schema);
					}
					catch
					{ }

					if (parsedRecord != null)
						yield return parsedRecord;
				}
			}
		}

		public static IEnumerable<dynamic> Parse(IEnumerable<RawPage> pages, IRawType[] schema)
		{
			return pages.SelectMany(x => x.Records).Select(x => Parse(x, schema));
		}

		public static IEnumerable<dynamic> Parse(IEnumerable<RawRecord> records, IRawType[] schema)
		{
			return records.Select(x => Parse(x, schema));
		}

		public static dynamic Parse(RawRecord record, IRawType[] schema)
		{
			return Parse(
				record.FixedLengthData != null ? record.FixedLengthData.ToArray() : null,
				record.VariableLengthOffsetValues != null ? record.VariableLengthOffsetValues.Select(x => x.ToArray()).ToArray() : null,
				record.NullBitmapRawBytes.ToArray(),
				schema
			);
		}
		
		public static dynamic Parse(byte[] fixedLengthData, byte[][] variableLengthData, byte[] nullBitmapBytes, IEnumerable<IRawType> schema)
		{
			var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			var nullBitmap = new BitArray(nullBitmapBytes);

			// Pointer to current read position from the fixed length data
			int fixedIndex = 0;

			// Current variable offset array entry index
			int variableIndex = 0;

			// Current bit byte being processed
			byte? bitByte = null;

			// Current bit byte index
			int bitByteBitIndex = 8;

			// Current null bitmap index
			int nullBitmapIndex = 0;

			foreach (var type in schema)
			{
				object value = null;

				// Is it a fixed length or a variable length type?
				if (type is IRawFixedLengthType)
				{
					var fixedType = (IRawFixedLengthType)type;

					if (fixedType is RawBit)
					{
						// Bits need to special care since they don't always consume bytes from the fixed length data stream
						if (bitByteBitIndex == 8)
						{
							bitByte = fixedLengthData.Skip(fixedIndex).Take(1).Single();
							fixedIndex++;
							bitByteBitIndex = 0;
						}

						value = (bitByte & (1 << bitByteBitIndex++)) != 0;
					}
					else
					{
						// Whereas any other fixed length column type is straightforward
						value = fixedType.GetValue(fixedLengthData.Skip(fixedIndex).Take(fixedType.Length).ToArray());
						fixedIndex += fixedType.Length;
					}
				}
				else
				{
					var variableType = (IRawVariableLengthType)type;

					// We may have schema columns that haven't been persisted, in which case we'll simply miss certain
					// variable length offset entries from the record completely. These can only be at the end. If we're
					// missing a value, it's an implicit null (ignoring 2012's ability to have non-persisted default values).
					if (variableLengthData != null && variableIndex < variableLengthData.Length)
					{
						value = variableType.GetValue(variableLengthData[variableIndex++]);
					}
					else
					{
						// If there is no variable length data for this column, and it's not null, it's empty
						value = variableType.EmptyValue;
					}
				}

				// If null bitmap indicates a null value, overwrite the previously found value
				if (nullBitmap[nullBitmapIndex++])
					value = null;

				result.Add(type.Name, value);
			}

			return new DynamicRow(result);
		}
	}
}