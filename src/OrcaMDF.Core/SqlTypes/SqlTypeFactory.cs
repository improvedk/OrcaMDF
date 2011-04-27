using System;
using OrcaMDF.Core.Common;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.SqlTypes
{
	public class SqlTypeFactory
	{
		internal SqlBitReadState BitReadState;

		public ISqlType Create(string type)
		{
			var typeDesc = ColumnTypeDescriptionFactory.GetDescription(type);

			switch(typeDesc.Type)
			{
				case ColumnType.BigInt:
					return new SqlBigInt();

				case ColumnType.Bit:
					return new SqlBit(this);

				case ColumnType.Char:
					return new SqlChar((short) typeDesc.VariableFixedLength);

				case ColumnType.DateTime:
					return new SqlDateTime();

				case ColumnType.Int:
					return new SqlInt();

				case ColumnType.NChar:
					return new SqlNChar((short) typeDesc.VariableFixedLength);

				case ColumnType.NVarchar:
					return new SqlNVarchar();

				case ColumnType.Varchar:
					return new SqlVarchar();

			}

			throw new ArgumentException("Unsupported type: " + type);
		}
	}
}