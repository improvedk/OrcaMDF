using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.Framework;

namespace OrcaMDF.Core.Engine.Pages
{
	public class PrimaryRecordPage : RecordPage
	{
		public PrimaryRecord[] Records { get; protected set; }

		public PrimaryRecordPage(byte[] bytes, Database database)
			: base(bytes, database)
		{
			parseRecords();
		}

		private void parseRecords()
		{
			Records = new PrimaryRecord[Header.SlotCnt];
			
			int cnt = 0;
			foreach (short recordOffset in SlotArray)
				Records[cnt++] = new PrimaryRecord(ArrayHelper.SliceArray(RawBytes, recordOffset, RawBytes.Length - recordOffset), this);
		}
	}
}