using System;
using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public static class SqlTypeFactory
	{
		public static ISqlType Create(ColumnTypeDescription typeDesc, RecordReadState readState)
		{
			switch(typeDesc.Type)
			{
				case ColumnType.Binary:
					return new SqlBinary((short)typeDesc.VariableFixedLength);

				case ColumnType.BigInt:
					return new SqlBigInt();

				case ColumnType.Bit:
					return new SqlBit(readState);

				case ColumnType.Char:
					return new SqlChar((short)typeDesc.VariableFixedLength);

				case ColumnType.DateTime:
					return new SqlDateTime();

				case ColumnType.Int:
					return new SqlInt();

				case ColumnType.NChar:
					return new SqlNChar((short)typeDesc.VariableFixedLength);

				case ColumnType.NVarchar:
					return new SqlNVarchar();

				case ColumnType.SmallInt:
					return new SqlSmallInt();

				case ColumnType.TinyInt:
					return new SqlTinyInt();

				case ColumnType.VarBinary:
					return new SqlVarBinary();

				case ColumnType.Varchar:
					return new SqlVarchar();
			}

			throw new ArgumentException("Unsupported type: " + typeDesc);
		}
	}
}