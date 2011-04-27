using System;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Common
{
	public class ColumnTypeDescriptionFactory
	{
		public static ColumnTypeDescription GetDescription(string type)
		{
			switch(type.Split('(')[0])
			{
				case "bigint":
					return new ColumnTypeDescription(ColumnType.BigInt, null);

				case "bit":
					return new ColumnTypeDescription(ColumnType.Bit, null);

				case "char":
					return new ColumnTypeDescription(ColumnType.Char, Convert.ToInt16(type.Split('(')[1].Split(')')[0]));

				case "ncar":
					return new ColumnTypeDescription(ColumnType.NChar, Convert.ToInt16(type.Split('(')[1].Split(')')[0]));

				case "datetime":
					return new ColumnTypeDescription(ColumnType.DateTime, null);

				case "int":
					return new ColumnTypeDescription(ColumnType.Int, null);

				case "varchar":
					return new ColumnTypeDescription(ColumnType.Varchar, null);

				case "nvarchar":
					return new ColumnTypeDescription(ColumnType.NVarchar, null);
			}

			throw new ArgumentException("Unsupported type: " + type);
		}
	}
}