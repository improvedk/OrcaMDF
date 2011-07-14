using System;

namespace OrcaMDF.Core.MetaData
{
	public class DataColumn
	{
		public short? MaxLength;
		public short? VariableFixedLength;
		public string Name;
		public ColumnType Type;
		public string TypeString;
		public bool IsNullable;
		public bool IsIncluded;
		public bool IsVariableLength;

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
					IsVariableLength = true;
					break;

				case "smallint":
					Type = ColumnType.SmallInt;
					break;

				case "tinyint":
					Type = ColumnType.TinyInt;
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