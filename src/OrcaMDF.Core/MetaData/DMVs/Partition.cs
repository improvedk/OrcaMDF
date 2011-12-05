using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Partition : Row
	{
		private const string CACHE_KEY = "DMV_Partition";

		private static readonly ISchema schema = new Schema(new[]
		    {
		        new DataColumn("PartitionID", "bigint"),
				new DataColumn("ObjectID", "int"),
				new DataColumn("IndexID", "int"),
				new DataColumn("PartitionNumber", "int"),
				new DataColumn("HobtID", "bigint"),
				new DataColumn("Rows", "bigint"),
				new DataColumn("FilestreamFilegroupID", "smallint"),
				new DataColumn("DataCompression", "tinyint"),
				new DataColumn("DataCompressionDesc", "nvarchar", true)
		    });

		public long PartitionID { get { return Field<long>("PartitionID"); } private set { this["PartitionID"] = value; } }
		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public int IndexID { get { return Field<int>("IndexID"); } private set { this["IndexID"] = value; } }
		public int PartitionNumber { get { return Field<int>("PartitionNumber"); } private set { this["PartitionNumber"] = value; } }
		public long HobtID { get { return Field<long>("HobtID"); } private set { this["HobtID"] = value; } }
		public long Rows { get { return Field<long>("Rows"); } private set { this["Rows"] = value; } }
		public short FilestreamFilegroupID { get { return Field<short>("FilestreamFilegroupID"); } private set { this["FilestreamFilegroupID"] = value; } }
		public byte DataCompression { get { return Field<byte>("DataCompression"); } private set { this["DataCompression"] = value; } }
		public string DataCompressionDesc { get { return Field<string>("DataCompressionDesc"); } private set { this["DataCompressionDesc"] = value; } }
		
		public Partition() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new Partition();
		}

		internal static IEnumerable<Partition> GetDmvData(Database db)
		{
			if (!db.ObjectCache.ContainsKey(CACHE_KEY))
			{
				db.ObjectCache[CACHE_KEY] = db.BaseTables.sysrowsets
					.Select(rs => new Partition
						{
							PartitionID = rs.rowsetid,
							ObjectID = rs.idmajor,
							IndexID = rs.idminor,
							PartitionNumber = rs.numpart,
							HobtID = rs.rowsetid,
							Rows = 0, // TODO
							FilestreamFilegroupID = rs.fgidfs,
							DataCompression = rs.cmprlevel,
							DataCompressionDesc = db.BaseTables.syspalvalues
								.Where(cl => cl.@class == "CMPL" && cl.value == rs.cmprlevel)
								.Select(cl => cl.name)
								.SingleOrDefault(),
						})
					.ToList();
			}

			return (IEnumerable<Partition>)db.ObjectCache[CACHE_KEY];
		}
	}
}