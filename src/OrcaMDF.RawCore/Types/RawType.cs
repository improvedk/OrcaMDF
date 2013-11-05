namespace OrcaMDF.RawCore.Types
{
	public abstract class RawType : IRawType
	{
		public string Name { get; protected set; }

		protected RawType(string name)
		{
			Name = name;
		}

		public static RawBit Bit(string name) { return new RawBit(name); }
		public static RawChar Char(string name, short length) { return new RawChar(name, length); }
		public static RawDate Date(string name) { return new RawDate(name); }
		public static RawDateTime DateTime(string name) { return new RawDateTime(name); }
		public static RawDecimal Decimal(string name, byte precision, byte scale) { return new RawDecimal(name, precision, scale); }
		public static RawHierarchyID HierarchyID(string name) { return new RawHierarchyID(name); }
		public static RawInt Int(string name) { return new RawInt(name); }
		public static RawMoney Money(string name) { return new RawMoney(name); }
		public static RawNChar NChar(string name, short length) { return new RawNChar(name, length); }
		public static RawNVarchar NVarchar(string name) { return new RawNVarchar(name); }
		public static RawSmallInt SmallInt(string name) { return new RawSmallInt(name); }
		public static RawTinyInt TinyInt(string name) { return new RawTinyInt(name); }
		public static RawUniqueIdentifier UniqueIdentifier(string name) { return new RawUniqueIdentifier(name); }
		public static RawVarBinary VarBinary(string name) { return new RawVarBinary(name); }
		public static RawVarchar Varchar(string name) { return new RawVarchar(name); }
		public static RawXml Xml(string name) { return new RawXml(name); }

		public abstract object GetValue(byte[] bytes);
	}
}