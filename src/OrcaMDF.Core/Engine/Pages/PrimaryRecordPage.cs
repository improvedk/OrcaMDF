using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.Framework;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class PrimaryRecordPage : RecordPage
	{
		internal PrimaryRecord[] Records { get; set; }

		protected CompressionContext CompressionContext;

		internal PrimaryRecordPage(byte[] bytes, CompressionContext compression, Database database)
			: base(bytes, database)
		{
			CompressionContext = compression;

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