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
		internal byte[] RawBytes;

		public IEnumerable<short> SlotArray
		{
			get
			{
				int pageEndIndex = 8192;

				for (var i = 1; i <= Header.SlotCnt; i++)
					yield return BitConverter.ToInt16(RawBytes, pageEndIndex - i * 2);
			}
		}

		public IEnumerable<RawRecord> BestEffortRecords
		{
			get
			{
				foreach (var entry in SlotArray)
				{
					RawRecord record = null;

					try
					{
						var type = RecordTypeParser.Parse(RawBytes[entry]);
						var recordBytes = new ArrayDelimiter<byte>(RawBytes, entry, RawBytes.Length - entry);

						switch (type)
						{
							case RecordType.Primary:
								record = new RawPrimaryRecord(recordBytes);
								break;

							case RecordType.Index:
								record = new RawIndexRecord(recordBytes, Header.Pminlen, Header.Level);
								break;

							default:
								record = new RawRecord(recordBytes);
								break;
						}
					}
					catch
					{ }

					if (record != null)
						yield return record;
				}
			}
		}

		public IEnumerable<RawRecord> Records
		{
			get
			{
				foreach (var entry in SlotArray)
				{
					// Get the record type from the first byte of the record, the A status byte
					var type = RecordTypeParser.Parse(RawBytes[entry]);
					var recordBytes = new ArrayDelimiter<byte>(RawBytes, entry, RawBytes.Length - entry);
					
					switch (type)
					{
						case RecordType.Primary:
							yield return new RawPrimaryRecord(recordBytes);
							break;

						case RecordType.Index:
							yield return new RawIndexRecord(recordBytes, Header.Pminlen, Header.Level);
							break;

						default:
							yield return new RawRecord(recordBytes);
							break;
					}
				}
			}
		}

		public RawPage(int pageID, byte[] rawBytes)
		{
			PageID = pageID;
			Header = new RawPageHeader(this);
			RawBytes = rawBytes;
		}
	}
}