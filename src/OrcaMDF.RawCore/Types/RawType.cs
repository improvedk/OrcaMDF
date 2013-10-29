namespace OrcaMDF.RawCore.Types
{
	public abstract class RawType : IRawType
	{
		public string Name { get; protected set; }

		protected RawType(string name)
		{
			Name = name;
		}

		public static RawInt Int(string name) { return new RawInt(name); }
		public static RawTinyint Tinyint(string name) { return new RawTinyint(name); }
		public static RawDatetime Datetime(string name) { return new RawDatetime(name); }
		public static RawNChar NChar(string name, short length) { return new RawNChar(name, length); }
		public static RawNVarchar NVarchar(string name) { return new RawNVarchar(name); }
		public static RawUniqueIdentifier UniqueIdentifier(string name) { return new RawUniqueIdentifier(name); }

		public abstract object GetValue(byte[] bytes);
	}
}