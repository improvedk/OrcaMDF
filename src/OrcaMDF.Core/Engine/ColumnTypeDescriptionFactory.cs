using System;

namespace OrcaMDF.Core.Engine
{
	public class ColumnTypeDescriptionFactory
	{
		public static ColumnTypeDescription GetDescription(string type)
		{
			switch(type.Split('(')[0])
			{
				case "bigint":
					return new ColumnTypeDescription(ColumnType.BigInt, null);

				case "binary":
					return new ColumnTypeDescription(ColumnType.Binary, Convert.ToInt16(type.Split('(')[1].Split(')')[0]));

				case "bit":
					return new ColumnTypeDescription(ColumnType.Bit, null);

				case "char":
					return new ColumnTypeDescription(ColumnType.Char, Convert.ToInt16(type.Split('(')[1].Split(')')[0]));

				case "datetime":
					return new ColumnTypeDescription(ColumnType.DateTime, null);

				case "int":
					return new ColumnTypeDescription(ColumnType.Int, null);

				case "ncar":
					return new ColumnTypeDescription(ColumnType.NChar, Convert.ToInt16(type.Split('(')[1].Split(')')[0]));

				case "nvarchar":
					return new ColumnTypeDescription(ColumnType.NVarchar, null);

				case "smallint":
					return new ColumnTypeDescription(ColumnType.SmallInt, null);

				case "tinyint":
					return new ColumnTypeDescription(ColumnType.TinyInt, null);

				case "varchar":
					return new ColumnTypeDescription(ColumnType.Varchar, null);
			}

			throw new ArgumentException("Unsupported type: " + type);
		}
	}
}