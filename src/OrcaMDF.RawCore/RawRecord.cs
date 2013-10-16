using OrcaMDF.Core.Engine.Records;
using System;
using System.Collections;

namespace OrcaMDF.RawCore
{
	public class RawRecord
	{
		private readonly int index;
		private readonly RawPage page;
		private readonly RawDatabase db;

		public bool HasNullBitmap { get; private set; }
		public RecordType Type { get; private set; }
		public bool HasVariableLengthColumns { get; private set; }
		public bool HasVersioningInformation { get; private set; }
		public bool Version { get; private set; }

		public byte RawStatusByteA
		{
			get { return db.Data[page.FileID][index]; }
		}

		public RawRecord(int index, RawPage page, RawDatabase db)
		{
			this.index = index;
			this.page = page;
			this.db = db;

			performMinimalParse();
		}

		private void performMinimalParse()
		{
			var bits = new BitArray(new [] { RawStatusByteA });

			Version = bits[0];
			HasNullBitmap = bits[4];
			HasVariableLengthColumns = bits[5];
			HasVersioningInformation = bits[6];
			Type = (RecordType)((Convert.ToByte(bits[1])) + (Convert.ToByte(bits[2]) << 1) + (Convert.ToByte(bits[3]) << 2));
		}
	}
}