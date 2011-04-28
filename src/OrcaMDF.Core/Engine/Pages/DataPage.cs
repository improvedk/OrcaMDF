using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.Engine.SqlTypes;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.Pages
{
	public class DataPage : RecordPage
	{
		public DataPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{ }

		public IList<T> GetEntities<T>() where T : new()
		{
			var result = new List<T>();

			for (int i = 0; i < Records.Length; i++)
			{
				var entity = new T();
				short fixedOffset = 0;
				short variableColumnIndex = 0;
				var record = Records[i];
				int columnIndex = 0;
				var readState = new RecordReadState();

				foreach (PropertyInfo prop in typeof (T).GetProperties())
				{
					ColumnAttribute column = Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute)) as ColumnAttribute;

					if(column != null)
					{
						var sqlType = SqlTypeFactory.Create(column.Description, readState);
						object columnValue = null;

						if(sqlType.IsVariableLength)
						{
							if(!record.NullBitmap[columnIndex])
								columnValue = sqlType.GetValue(record.VariableLengthColumnData[variableColumnIndex]);

							variableColumnIndex++;
						}
						else
						{
							// Must cache type FixedLength as it may change after getting a value (e.g. SqlBit)
							short fixedLength = sqlType.FixedLength.Value;

							if (!record.NullBitmap[columnIndex])
								columnValue = sqlType.GetValue(record.FixedLengthData.Skip(fixedOffset).Take(fixedLength).ToArray());

							fixedOffset += fixedLength;
						}

						columnIndex++;
						prop.SetValue(entity, columnValue, null);
					}
				}

				result.Add(entity);
			}

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder(base.ToString());
			sb.AppendLine();
			sb.AppendLine("Slot Array:");
			sb.AppendLine("Row\tOffset");
			for (int i = 0; i < Header.SlotCnt; i++)
				sb.AppendLine(i + "\t" + SlotArray[i]);

			return sb.ToString();
		}
	}
}