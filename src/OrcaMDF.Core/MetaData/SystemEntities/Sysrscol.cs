using System;

namespace OrcaMDF.Core.MetaData.SystemEntities
{
	/// <summary>
	/// Matches sys.sysrscols
	/// </summary>
	public class Sysrscol : Row
	{
		public Sysrscol()
		{
			Columns.Add(new DataColumn("rsid", "bigint"));
			Columns.Add(new DataColumn("rscolid", "int"));
			Columns.Add(new DataColumn("hbcolid", "int"));
			Columns.Add(new DataColumn("rcmodified", "bigint"));
			Columns.Add(new DataColumn("ti", "int"));
			Columns.Add(new DataColumn("cid", "int"));
			Columns.Add(new DataColumn("ordkey", "smallint"));
			Columns.Add(new DataColumn("maxinrowlen", "smallint"));
			Columns.Add(new DataColumn("status", "int"));
			Columns.Add(new DataColumn("offset", "int"));
			Columns.Add(new DataColumn("nullbit", "int"));
			Columns.Add(new DataColumn("bitpos", "smallint"));
			Columns.Add(new DataColumn("colguid", "varbinary(16)", true));
			Columns.Add(new DataColumn("dbfragid", "int"));
		}

		public override Row NewRow()
		{
			return new Sysrscol();
		}

		public long RowsetID { get { return Field<long>("rsid"); } }
		public int PartitionColumnID { get { return Field<int>("rscolid"); } }
		public int HBColID { get { return Field<int>("hbcolid"); } }
		public long ModifiedCount { get { return Field<long>("rcmodified"); } }
		public int TI { get { return Field<int>("ti"); } }
		public int CID { get { return Field<int>("cid"); } }
		public short KeyOrdinal { get { return Field<short>("ordkey"); } }
		public short MaxInRowLen { get { return Field<short>("maxinrowlen"); } }
		public int Status { get { return Field<int>("status"); } }
		public int Offset { get { return Field<int>("offset"); } }
		public int NullBit { get { return Field<int>("nullbit"); } }
		public short BitPosition { get { return Field<short>("bitpos"); } }
		public byte[] ColumnGuid { get { return Field<byte[]>("colguid"); } }
		public int DBFragID { get { return Field<int>("dbfragid"); } }

		// Calculated fields
		public bool IsReplicated { get { return Convert.ToBoolean(Status & 1); } }
		public bool IsLoggedForReplication { get { return Convert.ToBoolean(Status & 4); } }
		public bool IsDropped { get { return Convert.ToBoolean(Status & 2); } }
		public bool IsFilestream { get { return Convert.ToBoolean(Status & 32); } }
		public bool IsNullable { get { return Convert.ToBoolean(1 - (Status & 128) / 128); } }
		public bool IsDescendingKey { get { return Convert.ToBoolean(Status & 8); } }
		public bool IsUniquifier { get { return Convert.ToBoolean(Status & 16); } }
		public short LeafOffset { get { return BitConverter.ToInt16(BitConverter.GetBytes(Offset & 0xFFFF), 0); } }
		public short InternalOffset { get { return Convert.ToInt16(Offset >> 16); } }
		public byte LeafBitPosition { get { return Convert.ToByte(BitPosition & 0xFF); } }
		public byte InternalBitPosition { get { return Convert.ToByte(BitPosition / 0x100); } }
		public short LeafNullBit { get { return Convert.ToInt16(NullBit & 0xFFFF); } }
		public short InternalNullBit { get { return Convert.ToInt16(NullBit >> 16); } }
		public bool IsAntiMatter { get { return Convert.ToBoolean(Status & 64); } }
		public Guid? PartitionColumnGuid { get { return ColumnGuid != null ? (Guid?)(new Guid(ColumnGuid)) : null; } }
		public bool IsSparse { get { return Convert.ToBoolean(Status & 0x100); } }

		private SysrscolTIParser tiParser;
		private void parseTI()
		{
			if (tiParser == null)
				tiParser = new SysrscolTIParser(TI);
		}

		public int SystemTypeID
		{
			get
			{
				parseTI();
				return tiParser.TypeID;
			}
		}

		public byte Precision
		{
			get
			{
				parseTI();
				return tiParser.Precision;
			}
		}

		public byte Scale
		{
			get
			{
				parseTI();
				return tiParser.Scale;
			}
		}

		public short MaxLength
		{
			get
			{
				parseTI();
				return tiParser.MaxLength;
			}
		}
	}
}