using System;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Engine.Records.VariableLengthDataProxies
{
	public class OverflowDataProxy : DataProxy, IVariableLengthDataProxy
	{
		private byte complexColumnType;
		private short indexLevel;
		private byte unused;
		private int sequence;
		private long timestamp;
		private byte[] data;

		public OverflowDataProxy(Page page, byte[] data)
			: base(page)
		{
			this.data = data;

			// Parsed according to table 7-1 (p. 378) in [SQL Server 2008 Internals]
			complexColumnType = data[0];
			indexLevel = BitConverter.ToInt16(data, 1);
			unused = data[3];
			sequence = BitConverter.ToInt32(data, 4);

			// Technically a 6-byte long value. Low two bytes always zero, thus not stored (http://bit.ly/mdAQpm)
			timestamp = BitConverter.ToUInt32(data, 8) << 16;
		}

		public byte[] GetBytes()
		{
			byte[] fieldData = new byte[0];
			for (int i = 12; i < data.Length; i += 12)
			{
				int length = BitConverter.ToInt32(data, i);
				int pageID = BitConverter.ToInt32(data, i + 4);
				short fileID = BitConverter.ToInt16(data, i + 8);
				short slot = BitConverter.ToInt16(data, i + 10);

				// Get referenced page
				var textMixPage = OriginPage.File.GetTextMixPage(new PagePointer(fileID, pageID));
				fieldData = fieldData.Concat(textMixPage.Records[slot].FixedLengthData).ToArray();
			}

			return fieldData;
		}
	}
}