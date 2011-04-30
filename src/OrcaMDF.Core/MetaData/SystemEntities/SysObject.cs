using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysObject
	{
		[Column("int")] // id
		public int ObjectID { get; internal set; }

		[Column("sysname")] // name
		public string Name { get; internal set; }

		[Column("int")] // nsid
		public int NSID { get; internal set; }

		[Column("tinyint")] // nsclass
		public byte NSClass { get; internal set; }

		[Column("int")] // status
		public int Status { get; internal set; }

		[Column("char(2)")] // type
		public string Type { get; internal set; }

		[Column("int")] // pid
		public int PID { get; internal set; }

		[Column("tinyint")] // pclass
		public byte PClass { get; internal set; }

		[Column("int")] // intprop
		public int IntProp { get; internal set; }

		[Column("datetime")] // created
		public DateTime Created { get; internal set; }

		[Column("datetime")] // modified
		public DateTime Modified { get; internal set; }

		public override string ToString()
		{
			return Name + " (" + Type + ", " + ObjectID + ")";
		}
	}
}