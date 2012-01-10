
namespace OrcaMDF.Core.Engine.Pages
{
	internal class ClusteredIndexPage : IndexRecordPage
	{
		public ClusteredIndexPage(byte[] bytes, Database database)
			: base(bytes, database)
		{ }
		/*
		public IEnumerable<T> GetEntities<T>() where T : ClusteredTableIndexRow, new()
		{
			for (int i = 0; i < Records.Length; i++)
			{
				short fixedOffset = 0;
				short variableColumnIndex = 0;
				var record = Records[i];
				int columnIndex = 0;
				var readState = new RecordReadState();
				var dataRow = new T();

				foreach (DataColumn col in dataRow.Columns)
				{
					var sqlType = SqlTypeFactory.Create(col, readState);
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

					// First row of leftmost index page has undefined values
					if (Header.PreviousPage == PagePointer.Zero && i == 0)
						dataRow[col] = null;
					else
						dataRow[col] = columnValue;
				}

				// At the end of the clustered index record we'll have a 6 byte page pointer
				dataRow.PagePointer = new PagePointer(record.FixedLengthData.Skip(fixedOffset).Take(6).ToArray());

				yield return dataRow;
			}
		}
		 */
	}
}