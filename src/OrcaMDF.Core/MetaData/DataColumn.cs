using System;

namespace OrcaMDF.Core.MetaData
{
	public class DataColumn
	{
		public short? MaxLength;
		public short? VariableFixedLength;
		public int? ColumnID;
		public string Name;
		public ColumnType Type;
		public byte Precision;
		public byte Scale;
		public string TypeString;
		public bool IsNullable;
		public bool IsIncluded;
		public bool IsVariableLength;
		public bool IsSparse;

		public DataColumn(string name, string type)
			: this(name, type, false)
		{ }

		public DataColumn(string name, string type, bool nullable)
		{
			Name = name;
			TypeString = type;
			IsNullable = nullable;

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

				case "decimal":
					Type = ColumnType.Decimal;

					var parts = type.Split('(')[1].Split(')')[0].Split(',');
					Precision = Convert.ToByte(parts[0].Trim());
					Scale = Convert.ToByte(parts[1].Trim());

					if (parts.Length == 3)
						IsVariableLength = Convert.ToBoolean(parts[2]);
					break;
				
				case "image":
					Type = ColumnType.Image;
					IsVariableLength = true;
					break;

				case "int":
					Type = ColumnType.Int;
					break;

				case "money":
					Type = ColumnType.Money;
					break;

				case "nchar":
					Type = ColumnType.NChar;
					VariableFixedLength = Convert.ToInt16(type.Split('(')[1].Split(')')[0]);
					break;

				case "ntext":
					Type = ColumnType.NText;
					IsVariableLength = true;
					break;

				case "nvarchar":
				case "sysname":
					Type = ColumnType.NVarchar;
					IsVariableLength = true;
					break;

				case "smalldatetime":
					Type = ColumnType.SmallDatetime;
					break;

				case "smallint":
					Type = ColumnType.SmallInt;
					break;

				case "smallmoney":
					Type = ColumnType.SmallMoney;
					break;

				case "text":
					Type = ColumnType.Text;
					IsVariableLength = true;
					break;

				case "tinyint":
					Type = ColumnType.TinyInt;
					break;

				case "uniqueidentifier":
					Type = ColumnType.UniqueIdentifier;
					break;

				case "uniquifier":
					Type = ColumnType.Uniquifier;
					IsVariableLength = true;
					break;

				case "rid":
					Type = ColumnType.RID;
					IsVariableLength = false;
					break;

				case "varbinary":
					Type = ColumnType.VarBinary;
					IsVariableLength = true;
					break;

				case "varchar":
					Type = ColumnType.Varchar;
					IsVariableLength = true;
					break;

				default:
					throw new ArgumentException("Unsupported type: " + type);
			}
		}

		/// <summary>
		/// Standard DataColumn to be used for uniquifier column
		/// </summary>
		public static DataColumn Uniquifier
		{
			get { return new DataColumn("___Uniquifier", "uniquifier"); }
		}

		/// <summary>
		/// Standard DataColumn to be used for rid column
		/// </summary>
		public static DataColumn RID
		{
			get { return new DataColumn("___RID", "rid"); }
		}

		public override string ToString()
		{
			return Name + " " + TypeString;
		}
	}
}
