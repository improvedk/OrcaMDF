using System;
using System.Data;

namespace OrcaMDF.Core.MetaData
{
	public class ColumnTypeDescription
	{
		public readonly ColumnType Type;
		public readonly short? VariableFixedLength;

		public ColumnTypeDescription(DataColumn col)
			: this(col.Caption, col, x => (short)((DataColumn)x).MaxLength)
		{ }

		public ColumnTypeDescription(string type)
			: this(type.Split('(')[0], type, x => Convert.ToInt16(x.ToString().Split('(')[1].Split(')')[0]))
		{ }

		private ColumnTypeDescription(string type, object typeOrigin, Func<object, short> getColumnLength)
		{
			switch (type)
			{
				case "bigint":
					Type = ColumnType.BigInt;
					break;

				case "binary":
					Type = ColumnType.Binary;
					VariableFixedLength = getColumnLength(typeOrigin);
					break;

				case "bit":
					Type = ColumnType.Bit;
					break;

				case "char":
					Type = ColumnType.Char;
					VariableFixedLength = getColumnLength(typeOrigin);
					break;

				case "datetime":
					Type = ColumnType.DateTime;
					break;

				case "int":
					Type = ColumnType.Int;
					break;

				case "ncar":
					Type = ColumnType.NChar;
					VariableFixedLength = getColumnLength(typeOrigin);
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
			return Type + "(" + VariableFixedLength + ")";
		}
	}
}