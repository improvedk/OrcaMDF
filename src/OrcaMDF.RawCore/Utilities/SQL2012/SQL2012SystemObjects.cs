using OrcaMDF.RawCore.Types;

namespace OrcaMDF.RawCore.Utilities.SQL2012
{
	public class SQL2012SystemObjects
	{
		public static SQL2012Sysschobjs sysschobjs = new SQL2012Sysschobjs();
		public static SQL2012Syscolpars syscolpars = new SQL2012Syscolpars();
	}

	public class SQL2012Syscolpars
	{
		public int ObjectID = 41;

		public IRawType[] Schema = {
			RawType.Int("id"),
			RawType.SmallInt("number"),
			RawType.Int("colid"),
			RawType.NVarchar("name"),
			RawType.TinyInt("xtype"),
			RawType.Int("utype"),
			RawType.SmallInt("length"),
			RawType.TinyInt("prec"),
			RawType.TinyInt("scale"),
			RawType.Int("collationid"),
			RawType.Int("status"),
			RawType.SmallInt("maxinrow"),
			RawType.Int("xmlns"),
			RawType.Int("dflt"),
			RawType.Int("chk"),
			RawType.VarBinary("idtval")
		};
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