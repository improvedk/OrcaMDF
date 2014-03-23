using OrcaMDF.RawCore.Types;

namespace OrcaMDF.RawCore.Utilities.SQL2012
{
	public class SQL2012Sysrowsets
	{
		public long rowsetid { get; private set; }
		public byte ownertype { get; private set; }
		public int idmajor { get; private set; }
		public int idminor { get; private set; }
		public int numpart { get; private set; }
		public int status { get; private set; }
		public short fgidfs { get; private set; }
		public long rcrows { get; private set; }
		public byte cmprlevel { get; private set; }
		public byte fillfact { get; private set; }
		public short maxnullbit { get; private set; }
		public int maxleaf { get; private set; }
		public short maxint { get; private set; }
		public short minleaf { get; private set; }
		public short minint { get; private set; }
		public byte[] rsguid { get; private set; }
		public byte[] lockres { get; private set; }
		public int? scope_id { get; private set; }

		public static int ObjectID = 5;

		public static IRawType[] Schema = {
			RawType.BigInt("rowsetid"),
			RawType.TinyInt("ownertype"),
			RawType.Int("idmajor"),
			RawType.Int("idminor"),
			RawType.Int("numpart"),
			RawType.Int("status"),
			RawType.SmallInt("fgidfs"),
			RawType.BigInt("rcrows"),
			RawType.TinyInt("cmprlevel"),
			RawType.TinyInt("fillfact"),
			RawType.SmallInt("maxnullbit"),
			RawType.Int("maxleaf"),
			RawType.SmallInt("maxint"),
			RawType.SmallInt("minleaf"),
			RawType.SmallInt("minint"),
			RawType.VarBinary("rsguid"),
			RawType.VarBinary("lockres"),
			RawType.Int("scope_id")
		};

		public static SQL2012Sysrowsets Row(dynamic obj)
		{
			return new SQL2012Sysrowsets {
				rowsetid = obj.rowsetid,
				ownertype = obj.ownertype,
				idmajor = obj.idmajor,
				idminor = obj.idminor,
				numpart = obj.numpart,
				status = obj.status,
				fgidfs = obj.fgidfs,
				rcrows = obj.rcrows,
				cmprlevel = obj.cmprlevel,
				fillfact = obj.fillfact,
				maxnullbit = obj.maxnullbit,
				maxleaf = obj.maxleaf,
				maxint = obj.maxint,
				minleaf = obj.minleaf,
				minint = obj.minint,
				rsguid = obj.rsguid,
				lockres = obj.lockres,
				scope_id = obj.scope_id
			};
		}
	}
}