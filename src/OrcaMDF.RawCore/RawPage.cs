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
		public RawSlotArray SlotArray { get; private set; }
		
		public IEnumerable<RawRecord> Records
		{
			get
			{
				foreach (var entry in SlotArray.Entries)
					yield return new RawRecord(entry, this, db);
			}
		}

		public RawPage(short fileID, int pageID, RawDatabase db)
		{
			this.db = db;
			FileID = fileID;
			PageID = pageID;
			Header = new RawPageHeader(this, db);
			SlotArray = new RawSlotArray(this, db);
		}
	}
}