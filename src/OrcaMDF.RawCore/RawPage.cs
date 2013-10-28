using OrcaMDF.RawCore.Records;
using System;
using System.Collections.Generic;

namespace OrcaMDF.RawCore
{
	public class RawPage
	{
		private RawDatabase db;

		public int DataFileIndex { get { return PageID * 8192; } }
		public int PageID { get; private set; }
		public short FileID { get; private set; }
		public RawPageHeader Header { get; private set; }
		
		public ArraySegment<byte> RawBytes
		{
			get { return new ArraySegment<byte>(db.Data[FileID], DataFileIndex, 8192); }
		}

		public IEnumerable<short> SlotArray
		{
			get
			{
				int pageEndIndex = DataFileIndex + 8192;

				for (var i = 1; i <= Header.SlotCnt; i++)
					yield return BitConverter.ToInt16(db.Data[FileID], pageEndIndex - i * 2);
			}
		}

		public IEnumerable<RawRecord> Records
		{
			get
			{
				foreach (var entry in SlotArray)
				{
					byte statusA = db.Data[FileID][DataFileIndex + entry];
					var type = (RawRecordType)((statusA & 0xE) >> 1);
					
					switch (type)
					{
						case RawRecordType.Primary:
							yield return new RawPrimaryRecord(DataFileIndex + entry, this, db);
							break;

						case RawRecordType.Index:
							yield return new RawIndexRecord(DataFileIndex + entry, this, db);
							break;

						default:
							yield return new RawRecord(DataFileIndex + entry, this, db);
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
		}
	}
}