using System;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.SqlTypes
{
	public class SqlTypeFactory
	{
		internal SqlBitReadState BitReadState;

		public ISqlType Create(ColumnTypeDescription typeDesc)
		{
			switch(typeDesc.Type)
			{
				case ColumnType.Binary:
					return new SqlBinary((short)typeDesc.VariableFixedLength);

				case ColumnType.BigInt:
					return new SqlBigInt();

				case ColumnType.Bit:
					return new SqlBit(this);

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

				case ColumnType.Varchar:
					return new SqlVarchar();
			}

			throw new ArgumentException("Unsupported type: " + typeDesc);
		}
	}
}