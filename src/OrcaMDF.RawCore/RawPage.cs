using OrcaMDF.Framework;
using OrcaMDF.RawCore.Records;
using System;
using System.Collections.Generic;

namespace OrcaMDF.RawCore
{
	public class RawPage
	{
		public int PageID { get; private set; }
		public RawPageHeader Header { get; private set; }
		public byte[] RawBytes;

		private readonly RawDataFile dataFile;
		
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
							yield return new RawPrimaryRecord(entry, this, dataFile);
							break;

						case RecordType.Index:
							yield return new RawIndexRecord(entry, this, dataFile);
							break;

						default:
							yield return new RawRecord(entry, this, dataFile);
							break;
					}
				}
			}
		}

		public RawPage(int pageID, RawDataFile dataFile)
		{
			this.dataFile = dataFile;
			PageID = pageID;
			Header = new RawPageHeader(this);
			RawBytes = dataFile.GetPageBytes(pageID);
		}
	}
}