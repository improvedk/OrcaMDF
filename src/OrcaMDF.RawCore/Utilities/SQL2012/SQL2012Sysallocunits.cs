using OrcaMDF.RawCore.Types;

namespace OrcaMDF.RawCore.Utilities.SQL2012
{
	public class SQL2012Sysallocunits
	{
		public long auid { get; private set; }
		public byte type { get; private set; }
		public long ownerid { get; private set; }
		public int status { get; private set; }
		public byte[] pgfirst { get; private set; }
		public byte[] pgroot { get; private set; }
		public byte[] pgfirstiam { get; private set; }
		public long pcused { get; private set; }
		public long pcdata { get; private set; }
		public long pcreserved { get; private set; }

		public static int ObjectID = 7;

		public static IRawType[] Schema = {
			RawType.BigInt("auid"),
			RawType.TinyInt("type"),
			RawType.BigInt("ownerid"),
			RawType.Int("status"),
			RawType.SmallInt("fgid"),
			RawType.Binary("pgfirst", 6),
			RawType.Binary("pgroot", 6),
			RawType.Binary("pgfirstiam", 6),
			RawType.BigInt("pcused"),
			RawType.BigInt("pcdata"),
			RawType.BigInt("pcreserved")
		};

		public static SQL2012Sysallocunits Row(dynamic obj)
		{
			return new SQL2012Sysallocunits {
				auid = obj.auid,
				type = obj.type,
				ownerid = obj.ownerid,
				status = obj.status,
				pgfirst = obj.pgfirst,
				pgroot = obj.pgroot,
				pgfirstiam = obj.pgfirstiam,
				pcused = obj.pcused,
				pcdata = obj.pcdata,
				pcreserved = obj.pcreserved
			};
		}
	}
}