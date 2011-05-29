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

		public IEnumerable<T> GetEntities<T>() where T : new()
		{
			for (int i = 0; i < Records.Length; i++)
			{
				var entity = new T();
				short fixedOffset = 0;
				short variableColumnIndex = 0;
				var record = Records[i];
				int columnIndex = 0;
				var readState = new RecordReadState();

				foreach(var col in ColumnAttribute.GetOrderedColumnProperties<T>())
				{
					var sqlType = SqlTypeFactory.Create(col.Column.Description, readState);
					object columnValue = null;

					if (sqlType.IsVariableLength)
					{
						if (!record.HasNullBitmap || !record.NullBitmap[columnIndex])
						{
							// If a nullable varlength column does not have a value, it may be not even appear in the varlength column array if it's at the tail
							if (record.VariableLengthColumnData == null || record.VariableLengthColumnData.Count <= variableColumnIndex)
								columnValue = sqlType.GetValue(new byte[] { });
							else
								columnValue = sqlType.GetValue(record.VariableLengthColumnData[variableColumnIndex]);
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
					col.Property.SetValue(entity, columnValue, null);
				}

				yield return entity;
			}
		}
	}
}