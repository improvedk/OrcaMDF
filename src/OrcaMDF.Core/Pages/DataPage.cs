using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using OrcaMDF.Core.SqlTypes;

namespace OrcaMDF.Core.Pages
{
	public class DataPage : Page
	{
		public short[] SlotArray { get; private set; }
		public Record[] Records { get; private set; }

		public DataPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseSlotArray();
			parseRecords();
		}

		private void parseRecords()
		{
			Records = new Record[SlotCnt];

			int cnt = 0;
			foreach(short recordOffset in SlotArray)
				Records[cnt++] = new Record(RawBytes.Skip(recordOffset).ToArray(), File);
		}

		private void parseSlotArray()
		{
			SlotArray = new short[SlotCnt];

			for(int i=0; i<SlotCnt; i++)
				SlotArray[i] = BitConverter.ToInt16(RawBytes, RawBytes.Length - i * 2 - 2);
		}

		public IList<T> GetRecords<T>() where T : new()
		{
			var result = new List<T>();

			for (int i = 0; i < Records.Length; i++)
			{
				var entity = new T();
				short fixedOffset = 0;
				short variableColumnIndex = 0;
				var record = Records[i];
				int columnIndex = 0;
				var sqlTypeFactory = new SqlTypeFactory();

				foreach (PropertyInfo prop in typeof (T).GetProperties())
				{
					ColumnAttribute column = Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute)) as ColumnAttribute;

					if(column != null)
					{
						var sqlType = sqlTypeFactory.Create(column.DbType);
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
			for (int i = 0; i < SlotCnt; i++)
				sb.AppendLine(i + "\t" + SlotArray[i]);

			return sb.ToString();
		}
	}
}