using OrcaMDF.Core.Engine.Records;
using OrcaMDF.RawCore.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.RawCore
{
	public class RawPage
	{
		private RawDatabase db;

		public int DataFileIndex { get { return PageID * 8192; } }
		public int PageID { get; private set; }
		public short FileID { get; private set; }
		public RawPageHeader Header { get; private set; }
		
		public IEnumerable<byte> RawBytes
		{
			get { return db.Data[FileID].Skip(DataFileIndex).Take(8192); }
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
					var type = (RecordType)((statusA & 0xE) >> 1);
					
					switch (type)
					{
						case RecordType.Primary:
							yield return new RawPrimaryRecord(DataFileIndex + entry, this, db);
							break;

						case RecordType.Index:
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