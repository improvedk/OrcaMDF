using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.Engine.SqlTypes;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.Pages
{
	public class DataPage : PrimaryRecordPage
	{
		public DataPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{ }

		public IEnumerable<Row> GetEntities(Row schema)
		{
			for (int i = 0; i < Records.Length; i++)
			{
				var record = Records[i];

				// Don't process forwarded blob fragments as they should only be processed from the referenced record
				if (record.Type == RecordType.BlobFragment)
					continue;

				short fixedOffset = 0;
				short variableColumnIndex = 0;
				short nullBitmapColumnIndex = 0;
				var readState = new RecordReadState();
				var dataRow = schema.NewRow();

				foreach (DataColumn col in dataRow.Columns)
				{
					var sqlType = SqlTypeFactory.Create(col, readState);
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
						if (sqlType.IsVariableLength)
						{
							if (!record.HasNullBitmap || !record.NullBitmap[nullBitmapColumnIndex])
							{
								// Records may not even have a variable length part even though null variable length columns exit in the schema
								if (record.HasVariableLengthColumns)
								{
									// If a nullable varlength column does not have a value, it may be not even appear in the varlength column array if it's at the tail
									if (record.VariableLengthColumnData == null || record.VariableLengthColumnData.Count <= variableColumnIndex)
										columnValue = sqlType.GetValue(new byte[] {});
									else
										columnValue = sqlType.GetValue(record.VariableLengthColumnData[variableColumnIndex]);
								}
							}

							variableColumnIndex++;
						}
						else
						{
							// Must cache type FixedLength as it may change after getting a value (e.g. SqlBit)
							short fixedLength = sqlType.FixedLength.Value;

							if (!record.HasNullBitmap || !record.NullBitmap[nullBitmapColumnIndex])
							{
								byte[] valueBytes = record.FixedLengthData.Skip(fixedOffset).Take(fixedLength).ToArray();

								// We may run out of fixed length bytes. In certain conditions a null integer may have been added without
								// there being a null bitmap. In such a case, we detect the null condition by there not being enough fixed
								// length bytes to process.
								if(valueBytes.Length > 0)
									columnValue = sqlType.GetValue(valueBytes);
							}

							fixedOffset += fixedLength;
						}

						// Sparse columns don't have an entry in the null bitmap, thus we should only increment it if the current
						// column was not a sparse column.
						nullBitmapColumnIndex++;
					}

					dataRow[col] = columnValue;
				}

				yield return dataRow;
			}
		}
	}
}