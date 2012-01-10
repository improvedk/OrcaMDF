using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.Engine.SqlTypes;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class NonclusteredIndexPage : IndexRecordPage
	{
		internal NonclusteredIndexPage(byte[] bytes, Database database)
			: base(bytes, database)
		{ }

		internal IEnumerable<Row> GetEntities(Row schema, CompressionContext compression)
		{
			for (int i = 0; i < Records.Length; i++)
			{
				var record = Records[i];

				short fixedOffset = 0;
				short variableColumnIndex = 0;
				int columnIndex = 0;
				var readState = new RecordReadState();
				var dataRow = schema.NewRow();

				foreach (DataColumn col in dataRow.Columns)
				{
					var sqlType = SqlTypeFactory.Create(col, readState, compression);
					object columnValue = null;

					if (sqlType.IsVariableLength)
					{
						if (!record.HasNullBitmap || !record.NullBitmap[columnIndex])
						{
							// If a nullable varlength column does not have a value, it may be not even appear in the varlength column array if it's at the tail
							if (record.VariableLengthColumnData.Count <= variableColumnIndex)
								columnValue = sqlType.GetValue(new byte[] { });
							else
								columnValue = sqlType.GetValue(record.VariableLengthColumnData[variableColumnIndex].GetBytes().ToArray());
						}

						variableColumnIndex++;
					}
					else
					{
						// Must cache type FixedLength as it may change after getting a value (e.g. SqlBit)
						short fixedLength = sqlType.FixedLength.Value;

						if (!record.HasNullBitmap || !record.NullBitmap[columnIndex])
							columnValue = sqlType.GetValue(record.FixedLengthData.Skip(fixedOffset).Take(fixedLength).ToArray());

						fixedOffset += fixedLength;
					}

					columnIndex++;
					dataRow[col] = columnValue;
				}

				yield return dataRow;
			}
		}
	}
}