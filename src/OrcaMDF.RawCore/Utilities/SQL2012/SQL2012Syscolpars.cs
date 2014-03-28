using OrcaMDF.RawCore.Types;

namespace OrcaMDF.RawCore.Utilities.SQL2012
{
	public class SQL2012Syscolpars
	{
		public int id { get; private set; }
		public short number { get; private set; }
		public int colid { get; private set; }
		public string name { get; private set; }
		public byte xtype { get; private set; }
		public int utype { get; private set; }
		public short length { get; private set; }
		public byte prec { get; private set; }
		public byte scale { get; private set; }
		public int collationid { get; private set; }
		public int status { get; private set; }
		public short maxinrow { get; private set; }
		public int xmlns { get; private set; }
		public int dflt { get; private set; }
		public int chk { get; private set; }
		public byte[] idtval { get; private set; }

		public static int ObjectID = 41;

		public static IRawType[] Schema = {
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

		public static SQL2012Syscolpars BestEffortRow(dynamic obj)
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

		public static SQL2012Syscolpars Row(dynamic obj)
		{
			return new SQL2012Syscolpars {
				id = obj.id,
				number = obj.number,
				colid = obj.colid,
				name = obj.name,
				xtype = obj.xtype,
				utype = obj.utype,
				length = obj.length,
				prec = obj.prec,
				scale = obj.scale,
				collationid = obj.collationid,
				status = obj.status,
				maxinrow = obj.maxinrow,
				xmlns = obj.xmlns,
				dflt = obj.dflt,
				chk = obj.chk,
				idtval = obj.idtval
			};
		}
	}
}