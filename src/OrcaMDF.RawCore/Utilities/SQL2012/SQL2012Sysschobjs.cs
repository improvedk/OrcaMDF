using System;
using OrcaMDF.RawCore.Types;

namespace OrcaMDF.RawCore.Utilities.SQL2012
{
	public class SQL2012Sysschobjs
	{
		public int id { get; private set; }
		public string name { get; private set; }
		public int nsid { get; private set; }
		public byte nsclass { get; private set; }
		public int status { get; private set; }
		public string type { get; private set; }
		public int pid { get; private set; }
		public byte pclass { get; private set; }
		public int intprop { get; private set; }
		public DateTime created { get; private set; }
		public DateTime modified { get; private set; }
		public int status2 { get; private set; }

		public static int ObjectID = 34;

		public static IRawType[] Schema = {
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

		public static SQL2012Sysschobjs BestEffortRow(dynamic obj)
		{
			try
			{
				return Row(obj);
			}
			catch
			{
				return null;
			}
		}

		public static SQL2012Sysschobjs Row(dynamic obj)
		{
			return new SQL2012Sysschobjs {
				id = obj.id,
				name = obj.name,
				nsid = obj.nsid,
				nsclass = obj.nsclass,
				status = obj.status,
				type = obj.type,
				pid = obj.pid,
				pclass = obj.pclass,
				intprop = obj.intprop,
				created = obj.created,
				modified = obj.modified,
				status2 = obj.status2
			};
		}
	}
}