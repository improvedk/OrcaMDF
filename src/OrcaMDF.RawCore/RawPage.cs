using OrcaMDF.Framework;
using OrcaMDF.RawCore.Records;
using System;
using System.Collections.Generic;

namespace OrcaMDF.RawCore
{
	public class RawPage
	{
		private RawDatabase db;

		public int PageID { get; private set; }
		public short FileID { get; private set; }
		public RawPageHeader Header { get; private set; }

		public byte[] RawBytes;
		
		public IEnumerable<short> SlotArray
		{
			get
			{
				int pageEndIndex = 8192;

				for (var i = 1; i <= Header.SlotCnt; i++)
					yield return BitConverter.ToInt16(RawBytes, pageEndIndex - i * 2);
			}
		}

		public IEnumerable<RawRecord> Records
		{
			get
			{
				foreach (var entry in SlotArray)
				{
					byte statusA = RawBytes[entry];
					var type = (RecordType)((statusA & 0xE) >> 1);
					
					switch (type)
					{
						case RecordType.Primary:
							yield return new RawPrimaryRecord(entry, this, db);
							break;

						case RecordType.Index:
							yield return new RawIndexRecord(entry, this, db);
							break;

						default:
							yield return new RawRecord(entry, this, db);
							break;
					}
				}
			}
		}

		public RawPage(short fileID, int pageID, RawDatabase db)
		{
			this.db = db;
			FileID = fileID;
			PageID = pageID;
			Header = new RawPageHeader(this, db);
			RawBytes = db.GetPageBytes(fileID, pageID);
		}
	}
}