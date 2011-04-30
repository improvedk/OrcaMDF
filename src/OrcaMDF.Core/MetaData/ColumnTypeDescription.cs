using System;

namespace OrcaMDF.Core.MetaData
{
	public class ColumnTypeDescription
	{
		public readonly ColumnType Type;
		public readonly short? VariableFixedLength;

		private readonly string typeString;

		public ColumnTypeDescription(string type)
		{
			typeString = type;

			switch (type.Split('(')[0])
			{
				case "bigint":
					Type = ColumnType.BigInt;
					break;

				case "binary":
					Type = ColumnType.Binary;
					VariableFixedLength = Convert.ToInt16(type.Split('(')[1].Split(')')[0]);
					break;

				case "bit":
					Type = ColumnType.Bit;
					break;

				case "char":
					Type = ColumnType.Char;
					VariableFixedLength = Convert.ToInt16(type.Split('(')[1].Split(')')[0]);
					break;

				case "datetime":
					Type = ColumnType.DateTime;
					break;

				case "int":
					Type = ColumnType.Int;
					break;

				case "ncar":
					Type = ColumnType.NChar;
					VariableFixedLength = Convert.ToInt16(type.Split('(')[1].Split(')')[0]);
					break;

				case "nvarchar":
				case "sysname":
					Type = ColumnType.NVarchar;
					break;

				case "smallint":
					Type = ColumnType.SmallInt;
					break;

				case "tinyint":
					Type = ColumnType.TinyInt;
					break;

				case "varbinary":
					Type = ColumnType.VarBinary;
					break;

				case "varchar":
					Type = ColumnType.Varchar;
					break;

				default:
					throw new ArgumentException("Unsupported type: " + type);
			}
		}

		public override string ToString()
		{
			return typeString;
		}
	}
}