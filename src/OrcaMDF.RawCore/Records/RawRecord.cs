using OrcaMDF.Framework;
using System.Collections;

namespace OrcaMDF.RawCore.Records
{
	public class RawRecord
	{
		protected readonly int Index;
		protected readonly RawPage Page;
		protected readonly RawDatabase DB;

		public bool HasNullBitmap { get; private set; }
		public RecordType Type { get; private set; }
		public bool HasVariableLengthColumns { get; private set; }
		public bool HasVersioningInformation { get; private set; }
		public bool Version { get; private set; }

		public byte RawStatusByteA
		{
			get { return Page.RawBytes[Index]; }
		}

		public RawRecord(int index, RawPage page, RawDatabase db)
		{
			this.Index = index;
			this.Page = page;
			this.DB = db;

			var bits = new BitArray(new[] { RawStatusByteA });

			Version = bits[0];
			HasNullBitmap = bits[4];
			HasVariableLengthColumns = bits[5];
			HasVersioningInformation = bits[6];
			Type = (RecordType)((RawStatusByteA & 0xE) >> 1);
		}
	}
}