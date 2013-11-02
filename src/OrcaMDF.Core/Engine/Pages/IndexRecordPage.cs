using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Framework;

namespace OrcaMDF.Core.Engine.Pages
{
	internal abstract class IndexRecordPage : RecordPage
	{
		internal IndexRecord[] Records { get; set; }

		protected IndexRecordPage(byte[] bytes, Database database)
			: base(bytes, database)
		{
			parseRecords();
		}

		private void parseRecords()
		{
			Records = new IndexRecord[Header.SlotCnt];

			int cnt = 0;
			foreach (short recordOffset in SlotArray)
				Records[cnt++] = new IndexRecord(ArrayHelper.SliceArray(RawBytes, recordOffset, RawBytes.Length - recordOffset), this);
		}
	}
}