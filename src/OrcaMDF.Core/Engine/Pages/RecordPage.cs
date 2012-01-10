using System;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class RecordPage : Page
	{
		public short[] SlotArray { get; private set; }

		internal RecordPage(byte[] bytes, Database database)
			: base(bytes, database)
		{
			parseSlotArray();
		}

		private void parseSlotArray()
		{
			SlotArray = new short[Header.SlotCnt];

			for (int i = 0; i < Header.SlotCnt; i++)
				SlotArray[i] = BitConverter.ToInt16(RawBytes, RawBytes.Length - i * 2 - 2);
		}
	}
}