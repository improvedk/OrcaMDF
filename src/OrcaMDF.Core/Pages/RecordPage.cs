using System;
using System.Linq;

namespace OrcaMDF.Core.Pages
{
	public abstract class RecordPage : Page
	{
		public short[] SlotArray { get; private set; }
		public Record[] Records { get; private set; }

		protected RecordPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseSlotArray();
			parseRecords();
		}

		private void parseRecords()
		{
			Records = new Record[SlotCnt];

			int cnt = 0;
			foreach (short recordOffset in SlotArray)
				Records[cnt++] = new Record(RawBytes.Skip(recordOffset).ToArray(), File);
		}

		private void parseSlotArray()
		{
			SlotArray = new short[SlotCnt];

			for (int i = 0; i < SlotCnt; i++)
				SlotArray[i] = BitConverter.ToInt16(RawBytes, RawBytes.Length - i * 2 - 2);
		}
	}
}