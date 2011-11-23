using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.Framework;

namespace OrcaMDF.Core.Engine.Pages
{
	public abstract class IndexRecordPage : RecordPage
	{
		public IndexRecord[] Records { get; protected set; }

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