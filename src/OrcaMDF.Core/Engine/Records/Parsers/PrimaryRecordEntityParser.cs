using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;
using OrcaMDF.Core.Engine.SqlTypes;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.Records.Parsers
{
	internal class PrimaryRecordEntityParser : RecordEntityParser
	{
		private readonly PrimaryRecordPage page;
		private readonly CompressionContext compression;

		internal PrimaryRecordEntityParser(PrimaryRecordPage page, CompressionContext compression)
		{
			this.page = page;
			this.compression = compression;
		}

		internal override IEnumerable<Row> GetEntities(Row schema)
		{
			foreach (var record in page.Records)
			{
				// Don't process forwarded blob fragments as they should only be processed from the referenced record
				if (record.Type == RecordType.BlobFragment)
					continue;

				short fixedOffset = 0;
				short variableColumnIndex = 0;
				short nonSparseColumnIndex = 0;
				var readState = new RecordReadState();
				var dataRow = schema.NewRow();

				foreach (DataColumn col in dataRow.Columns)
				{
					var sqlType = SqlTypeFactory.Create(col, readState, compression);
					object columnValue = null;

					// Sparse columns needs to retrieve their values from the sparse vector, contained in the very last
					// variable length column in the record.
					if (col.IsSparse)
					{
						// We may encounter records that don't have any sparse vectors, for instance if no sparse columns have values
						if (record.SparseVector != null)
						{
							// Column ID's are stored as ints in general. In the sparse vector though, they're stored as shorts.
							if (record.SparseVector.ColumnValues.ContainsKey((short)col.ColumnID))
								columnValue = sqlType.GetValue(record.SparseVector.ColumnValues[(short)col.ColumnID]);
						}
					}
					else
					{
						// Before we even try to parse the column & make a null bitmap lookup, ensure that it's present in the record.
						// There may be columns > record.NumberOfColumns caused by nullable columns added to the schema after the record was written.
						if (nonSparseColumnIndex < record.NumberOfColumns)
						{
							if (sqlType.IsVariableLength)
							{
								// If there's either no null bitmap, or the null bitmap defines the column as non-null.
								if (!record.HasNullBitmap || !record.NullBitmap[nonSparseColumnIndex])
								{
									// If the current variable length column index exceeds the number of stored
									// variable length columns, the value is empty by definition (that is, 0 bytes, but not null).
									if (variableColumnIndex < record.NumberOfVariableLengthColumns)
										columnValue = sqlType.GetValue(record.VariableLengthColumnData[variableColumnIndex].GetBytes().ToArray());
									else
										columnValue = sqlType.GetValue(new byte[0]);
								}

								variableColumnIndex++;
							}
							else
							{
								// Must cache type FixedLength as it may change after getting a value (e.g. SqlBit)
								short fixedLength = sqlType.FixedLength.Value;

								if (!record.HasNullBitmap || !record.NullBitmap[nonSparseColumnIndex])
								{
									byte[] valueBytes = record.FixedLengthData.Skip(fixedOffset).Take(fixedLength).ToArray();

									// We may run out of fixed length bytes. In certain conditions a null integer may have been added without
									// there being a null bitmap. In such a case, we detect the null condition by there not being enough fixed
									// length bytes to process.
									if (valueBytes.Length > 0)
										columnValue = sqlType.GetValue(valueBytes);
								}

								fixedOffset += fixedLength;
							}

							// Sparse columns don't have an entry in the null bitmap, thus we should only increment it if the current
							// column was not a sparse column.
							nonSparseColumnIndex++;
						}
					}

					dataRow[col] = columnValue;
				}

				yield return dataRow;
			}
		}

		internal override PagePointer NextPage
		{
			get { return page.Header.NextPage; }
		}
	}
}