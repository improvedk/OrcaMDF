using System.Linq;
using OrcaMDF.Core.Engine.Records;

namespace OrcaMDF.Core.Engine.Pages
{
	public abstract class IndexRecordPage : RecordPage
	{
		public IndexRecord[] Records { get; protected set; }

		protected IndexRecordPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseRecords();
		}

		private void parseRecords()
		{
			Records = new IndexRecord[Header.SlotCnt];

			int cnt = 0;
			foreach (short recordOffset in SlotArray)
				Records[cnt++] = new IndexRecord(RawBytes.Skip(recordOffset).ToArray(), this);
		}
	}
}