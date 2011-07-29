using System.Linq;
using OrcaMDF.Core.Engine.Records;

namespace OrcaMDF.Core.Engine.Pages
{
	public class TextMixPage : RecordPage
	{
		public TextRecord[] Records { get; protected set; }

		public TextMixPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseRecords();
		}

		private void parseRecords()
		{
			Records = new TextRecord[Header.SlotCnt];

			int cnt = 0;
			foreach (short recordOffset in SlotArray)
				Records[cnt++] = new TextRecord(RawBytes.Skip(recordOffset).ToArray(), this);
		}
	}
}