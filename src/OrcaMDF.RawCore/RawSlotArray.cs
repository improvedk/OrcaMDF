
using System;
using System.Collections.Generic;

namespace OrcaMDF.RawCore
{
	public class RawSlotArray
	{
		private readonly RawDatabase db;
		private readonly RawPage page;

		public IEnumerable<short> Entries
		{
			get
			{
				int pageEndIndex = page.DataFileIndex + 8192;

				for (var i = 1; i <= page.Header.SlotCnt; i++)
					yield return BitConverter.ToInt16(db.Data[page.FileID], pageEndIndex - i * 2);
			}
		}

		public RawSlotArray(RawPage page, RawDatabase db)
		{
			this.db = db;
			this.page = page;
		}
	}
}