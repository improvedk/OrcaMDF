using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysObject
	{
		[Column("int", 1)] // id
		public int ObjectID { get; internal set; }

		[Column("sysname", 2)] // name
		public string Name { get; internal set; }

		[Column("int", 3)] // nsid
		public int NSID { get; internal set; }

		[Column("tinyint", 4)] // nsclass
		public byte NSClass { get; internal set; }

		[Column("int", 4)] // status
		public int Status { get; internal set; }

		[Column("char(2)", 5)] // type
		public string Type { get; internal set; }

		[Column("int", 6)] // pid
		public int PID { get; internal set; }

		[Column("tinyint", 7)] // pclass
		public byte PClass { get; internal set; }

		[Column("int", 8)] // intprop
		public int IntProp { get; internal set; }

		[Column("datetime", 9)] // created
		public DateTime Created { get; internal set; }

		[Column("datetime", 10)] // modified
		public DateTime Modified { get; internal set; }

		public override string ToString()
		{
			return Name + " (" + Type + ", " + ObjectID + ")";
		}
	}
}