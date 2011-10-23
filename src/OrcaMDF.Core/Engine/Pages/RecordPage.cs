using System;

namespace OrcaMDF.Core.Engine.Pages
{
	public abstract class RecordPage : Page
	{
		public short[] SlotArray { get; private set; }

		protected RecordPage(byte[] bytes, Database database)
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