using System;
using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public static class SqlTypeFactory
	{
		public static ISqlType Create(DataColumn column, RecordReadState readState, CompressionContext compression)
		{
			switch(column.Type)
			{
				case ColumnType.Binary:
					return new SqlBinary((short)column.VariableFixedLength, compression);

				case ColumnType.BigInt:
					return new SqlBigInt(compression);

				case ColumnType.Bit:
					return new SqlBit(readState, compression);

				case ColumnType.Char:
					return new SqlChar((short)column.VariableFixedLength, compression);

				case ColumnType.DateTime:
					return new SqlDateTime(compression);

				case ColumnType.Decimal:
					return new SqlDecimal(column.Precision, column.Scale, compression);
				
				case ColumnType.Image:
					return new SqlImage(compression);

				case ColumnType.Int:
					return new SqlInt(compression);

				case ColumnType.Money:
					return new SqlMoney(compression);

				case ColumnType.NChar:
					return new SqlNChar((short)column.VariableFixedLength, compression);

				case ColumnType.NText:
					return new SqlNText(compression);

				case ColumnType.NVarchar:
					return new SqlNVarchar(compression);

				case ColumnType.RID:
					return new SqlRID(compression);

				case ColumnType.SmallDatetime:
					return new SqlSmallDateTime(compression);

				case ColumnType.SmallInt:
					return new SqlSmallInt(compression);

				case ColumnType.SmallMoney:
					return new SqlSmallMoney(compression);

				case ColumnType.Text:
					return new SqlText(compression);

				case ColumnType.TinyInt:
					return new SqlTinyInt(compression);

				case ColumnType.UniqueIdentifier:
					return new SqlUniqueIdentifier(compression);

				case ColumnType.Uniquifier:
					return new SqlUniquifier(compression);

				case ColumnType.VarBinary:
					return new SqlVarBinary(compression);

				case ColumnType.Varchar:
					return new SqlVarchar(compression);

				case ColumnType.Variant:
					return new SqlVariant(compression);
			}

			throw new ArgumentException("Unsupported type: " + column);
		}
	}
}
