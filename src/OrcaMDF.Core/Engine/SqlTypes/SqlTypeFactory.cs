using System;
using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public static class SqlTypeFactory
	{
		public static ISqlType Create(DataColumn column, RecordReadState readState)
		{
			switch(column.Type)
			{
				case ColumnType.Binary:
					return new SqlBinary((short)column.VariableFixedLength);

				case ColumnType.BigInt:
					return new SqlBigInt();

				case ColumnType.Bit:
					return new SqlBit(readState);

				case ColumnType.Char:
					return new SqlChar((short)column.VariableFixedLength);

				case ColumnType.DateTime:
					return new SqlDateTime();

				case ColumnType.Int:
					return new SqlInt();

				case ColumnType.NChar:
					return new SqlNChar((short)column.VariableFixedLength);

				case ColumnType.NVarchar:
					return new SqlNVarchar();

				case ColumnType.RID:
					return new SqlRID();

				case ColumnType.SmallInt:
					return new SqlSmallInt();

				case ColumnType.TinyInt:
					return new SqlTinyInt();

				case ColumnType.Uniquifier:
					return new SqlUniquifier();

				case ColumnType.VarBinary:
					return new SqlVarBinary();

				case ColumnType.Varchar:
					return new SqlVarchar();
			}

			throw new ArgumentException("Unsupported type: " + column);
		}
	}
}