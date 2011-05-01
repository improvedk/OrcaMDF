using System.Linq;
using OrcaMDF.Core.Engine.Records;

namespace OrcaMDF.Core.Engine.Pages
{
	public class PrimaryRecordPage : RecordPage
	{
		public PrimaryRecord[] Records { get; protected set; }

		public PrimaryRecordPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseRecords();
		}

		private void parseRecords()
		{
			Records = new PrimaryRecord[Header.SlotCnt];

			int cnt = 0;
			foreach (short recordOffset in SlotArray)
				Records[cnt++] = new PrimaryRecord(RawBytes.Skip(recordOffset).ToArray(), this);
		}
	}
}