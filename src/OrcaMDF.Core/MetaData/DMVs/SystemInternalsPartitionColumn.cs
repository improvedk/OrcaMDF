using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class SystemInternalsPartitionColumn
	{
		public long PartitionID { get; private set; }
		public int PartitionColumnID { get; private set; }
		public long ModifiedCount { get; private set; }
		public short MaxInrowLength { get; private set; }
		public bool IsReplicated { get; private set; }
		public bool IsLoggedForReplication { get; private set; }
		public bool IsDropped { get; private set; }
		public byte SystemTypeID { get; private set; }
		public short MaxLength { get; private set; }
		public byte Precision { get; private set; }
		public byte Scale { get; private set; }
		public string CollationName { get { throw new NotImplementedException(); } } // TODO: Get from CollationPropertyFromId(c.cid, 'name')
		public bool IsFilestream { get; private set; }
		public short KeyOrdinal { get; private set; }
		public bool IsNullable { get; private set; }
		public bool IsDescendingKey { get; private set; }
		public bool IsUniqueifier { get; private set; }
		public short LeafOffset { get; private set; }
		public short InternalOffset { get; private set; }
		public byte LeafBitPosition { get; private set; }
		public byte InternalBitPosition { get; private set; }
		public short LeafNullBit { get; private set; }
		public short InternalNullBit { get; private set; }
		public bool IsAntiMatter { get; private set; }
		public Guid? PartitionColumnGuid { get; private set; }
		public bool IsSparse { get; private set; }
		
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