using OrcaMDF.RawCore.Types;

namespace OrcaMDF.RawCore.Utilities.SQL2012
{
	public class SQL2012SystemObjects
	{
		public static SQL2012Sysschobjs sysschobjs = new SQL2012Sysschobjs();
	}

	public class SQL2012Sysschobjs
	{
		public int ObjectID = 34;

		public IRawType[] Schema = {
			RawType.Int("id"),
			RawType.Sysname("name"),
			RawType.Int("nsid"),
			RawType.TinyInt("nsclass"),
			RawType.Int("status"),
			RawType.Char("type", 2),
			RawType.Int("pid"),
			RawType.TinyInt("pclass"),
			RawType.Int("intprop"),
			RawType.DateTime("created"),
			RawType.DateTime("modified"),
			RawType.Int("status2")
		};
	}
}