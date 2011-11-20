using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SystemInternalsPartitionColumn : Row
	{
		public long PartitionID { get { return Field<long>("PartitionID"); } private set { this["PartitionID"] = value; } }
		public int PartitionColumnID { get { return Field<int>("PartitionColumnID"); } private set { this["PartitionColumnID"] = value; } }
		public long ModifiedCount { get { return Field<long>("ModifiedCount"); } private set { this["ModifiedCount"] = value; } }
		public short MaxInrowLength { get { return Field<short>("MaxInrowLength"); } private set { this["MaxInrowLength"] = value; } }
		public bool IsReplicated { get { return Field<bool>("IsReplicated"); } private set { this["IsReplicated"] = value; } }
		public bool IsLoggedForReplication { get { return Field<bool>("IsLoggedForReplication"); } private set { this["IsLoggedForReplication"] = value; } }
		public bool IsDropped { get { return Field<bool>("IsDropped"); } private set { this["IsDropped"] = value; } }
		public byte SystemTypeID { get { return Field<byte>("SystemTypeID"); } private set { this["SystemTypeID"] = value; } }
		public short MaxLength { get { return Field<short>("MaxLength"); } private set { this["MaxLength"] = value; } }
		public byte Precision { get { return Field<byte>("Precision"); } private set { this["Precision"] = value; } }
		public byte Scale { get { return Field<byte>("Scale"); } private set { this["Scale"] = value; } }
		public string CollationName { get { return Field<string>("CollationName"); } private set { this["CollationName"] = value; } } // TODO
		public bool IsFilestream { get { return Field<bool>("IsFilestream"); } private set { this["IsFilestream"] = value; } }
		public short KeyOrdinal { get { return Field<short>("KeyOrdinal"); } private set { this["KeyOrdinal"] = value; } }
		public bool IsNullable { get { return Field<bool>("IsNullable"); } private set { this["IsNullable"] = value; } }
		public bool IsDescendingKey { get { return Field<bool>("IsDescendingKey"); } private set { this["IsDescendingKey"] = value; } }
		public bool IsUniqueifier { get { return Field<bool>("IsUniqueifier"); } private set { this["IsUniqueifier"] = value; } }
		public short LeafOffset { get { return Field<short>("LeafOffset"); } private set { this["LeafOffset"] = value; } }
		public short InternalOffset { get { return Field<short>("InternalOffset"); } private set { this["InternalOffset"] = value; } }
		public byte LeafBitPosition { get { return Field<byte>("LeafBitPosition"); } private set { this["LeafBitPosition"] = value; } }
		public byte InternalBitPosition { get { return Field<byte>("InternalBitPosition"); } private set { this["InternalBitPosition"] = value; } }
		public short LeafNullBit { get { return Field<short>("LeafNullBit"); } private set { this["LeafNullBit"] = value; } }
		public short InternalNullBit { get { return Field<short>("InternalNullBit"); } private set { this["InternalNullBit"] = value; } }
		public bool IsAntiMatter { get { return Field<bool>("IsAntiMatter"); } private set { this["IsAntiMatter"] = value; } }
		public Guid? PartitionColumnGuid { get { return Field<Guid?>("PartitionColumnGuid"); } private set { this["PartitionColumnGuid"] = value; } }
		public bool IsSparse { get { return Field<bool>("IsSparse"); } private set { this["IsSparse"] = value; } }

		public SystemInternalsPartitionColumn()
		{
			Columns.Add(new DataColumn("PartitionID", "bigint"));
			Columns.Add(new DataColumn("PartitionColumnID", "int"));
			Columns.Add(new DataColumn("ModifiedCount", "bigint"));
			Columns.Add(new DataColumn("MaxInrowLength", "smallint", true));
			Columns.Add(new DataColumn("IsReplicated", "bit", true));
			Columns.Add(new DataColumn("IsLoggedForReplication", "bit", true));
			Columns.Add(new DataColumn("IsDropped", "bit", true));
			Columns.Add(new DataColumn("SystemTypeID", "tinyint", true));
			Columns.Add(new DataColumn("MaxLength", "smallint", true));
			Columns.Add(new DataColumn("Precision", "tinyint", true));
			Columns.Add(new DataColumn("Scale", "tinyint", true));
			Columns.Add(new DataColumn("CollationName", "sysname", true));
			Columns.Add(new DataColumn("IsFilestream", "bit", true));
			Columns.Add(new DataColumn("KeyOrdinal", "smallint"));
			Columns.Add(new DataColumn("IsNullable", "bit", true));
			Columns.Add(new DataColumn("IsDescendingKey", "bit", true));
			Columns.Add(new DataColumn("IsUniqueifier", "bit", true));
			Columns.Add(new DataColumn("LeafOffset", "smallint", true));
			Columns.Add(new DataColumn("InternalOffset", "smallint", true));
			Columns.Add(new DataColumn("LeafBitPosition", "tinyint", true));
			Columns.Add(new DataColumn("InternalBitPosition", "tinyint", true));
			Columns.Add(new DataColumn("LeafNullBit", "smallint", true));
			Columns.Add(new DataColumn("InternalNullBit", "smallint", true));
			Columns.Add(new DataColumn("IsAntiMatter", "bit", true));
			Columns.Add(new DataColumn("PartitionColumnGuid", "uniqueidentifier", true));
			Columns.Add(new DataColumn("IsSparse", "bit", true));
		}

		public override Row NewRow()
		{
			return new SystemInternalsPartitionColumn();
		}
		
		internal static IEnumerable<SystemInternalsPartitionColumn> GetDmvData(Database db)
		{
			return db.BaseTables.sysrscols
				.Select(c => new {c, ti = new SysrscolTIParser(c.ti)})
				.Select(x => new SystemInternalsPartitionColumn
				    {
				        PartitionID = x.c.rsid,
				        PartitionColumnID = x.c.rscolid,
				        ModifiedCount = x.c.rcmodified,
				        KeyOrdinal = x.c.ordkey,
				        SystemTypeID = x.ti.TypeID,
				        MaxLength = x.ti.MaxLength,
				        Precision = x.ti.Precision,
				        Scale = x.ti.Scale,
				        MaxInrowLength = x.c.maxinrowlen == 0 ? x.ti.MaxLength : x.c.maxinrowlen,
						LeafOffset = BitConverter.ToInt16(BitConverter.GetBytes(x.c.offset & 0xFFFF), 0),
						IsReplicated = Convert.ToBoolean(x.c.status & 1),
						IsLoggedForReplication = Convert.ToBoolean(x.c.status & 4),
						IsDropped = Convert.ToBoolean(x.c.status & 2),
						IsFilestream = Convert.ToBoolean(x.c.status & 32),
						IsNullable = Convert.ToBoolean(1 - (x.c.status & 128) / 128),
						IsDescendingKey = Convert.ToBoolean(x.c.status & 8),
						IsUniqueifier = Convert.ToBoolean(x.c.status & 16),
						LeafBitPosition = Convert.ToByte(x.c.bitpos & 0xFF),
						InternalBitPosition = Convert.ToByte(x.c.bitpos / 0x100),
						IsSparse = Convert.ToBoolean(x.c.status & 0x100),
						IsAntiMatter = Convert.ToBoolean(x.c.status & 64),
						InternalOffset = BitConverter.ToInt16(BitConverter.GetBytes((x.c.status & 0xFFFF0000) >> 16), 0),
						PartitionColumnGuid = x.c.colguid != null ? (Guid?)(new Guid(x.c.colguid)) : null,
						InternalNullBit = BitConverter.ToInt16(BitConverter.GetBytes((x.c.nullbit & 0xFFFF0000) >> 16), 0),
						LeafNullBit = BitConverter.ToInt16(BitConverter.GetBytes(x.c.nullbit & 0xFFFF), 0)
				    });
		}
	}
}