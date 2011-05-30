using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	public class SysScalarType
	{
		[Column("int", 1)]
		public int ID { get; internal set; }

		[Column("int", 2)]
		public int SchemaID { get; internal set; }

		[Column("sysname", 3)]
		public string Name { get; internal set; }

		[Column("tinyint", 4)]
		public byte XType { get; internal set; }

		[Column("smallint", 5)]
		public short Length { get; internal set; }

		[Column("tinyint", 6)]
		public byte Precision { get; internal set; }

		[Column("tinyint", 7)]
		public byte Scale { get; internal set; }

		[Column("int", 8)]
		public int CollationID { get; internal set; }

		[Column("int", 9)]
		public int Status { get; internal set; }

		[Column("datetime", 10)]
		public DateTime Created { get; internal set; }

		[Column("datetime", 11)]
		public DateTime Modified { get; internal set; }

		[Column("int", 12)]
		public int Dflt { get; internal set; }

		[Column("int", 13)]
		public int Chk { get; internal set; }

		public bool IsNullable
		{
			get { return Convert.ToBoolean(1 - (Status & 1)); }
		}

		public bool AnsiPadded
		{
			get { return Convert.ToBoolean(Status & 2); }
		}

		public bool IsRowGuidCol
		{
			get { return Convert.ToBoolean(Status & 8); }
		}

		public bool IsIdentity
		{
			get { return Convert.ToBoolean(Status & 4); }
		}

		public bool IsComputed
		{
			get { return Convert.ToBoolean(Status & 16); }
		}

		public bool IsFilestream
		{
			get { return Convert.ToBoolean(Status & 32); }
		}

		public bool IsReplicated
		{
			get { return Convert.ToBoolean(Status & 0x020000); }
		}

		public bool IsNonSqlSubscribed
		{
			get { return Convert.ToBoolean(Status & 0x040000); }
		}

		public bool IsMergePublished
		{
			get { return Convert.ToBoolean(Status & 0x080000); }
		}

		public bool IsDtsReplicated
		{
			get { return Convert.ToBoolean(Status & 0x100000); }
		}

		public bool IsXmlDocument
		{
			get { return Convert.ToBoolean(Status & 2048); }
		}
	}
}