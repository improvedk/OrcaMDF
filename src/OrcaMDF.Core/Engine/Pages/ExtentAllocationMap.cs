using System.Collections;
using System.Collections.Generic;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class ExtentAllocationMap : PrimaryRecordPage
	{
		protected bool[] ExtentMap = new bool[63904];

		public ExtentAllocationMap(byte[] bytes, Database database)
			: base(bytes, CompressionContext.NoCompression, database)
		{
			parseBitmap();
		}

		private void parseBitmap()
		{
			byte[] bitmap = Records[1].FixedLengthData;

			int index = 0;

			foreach (byte b in bitmap)
			{
				var ba = new BitArray(new[] {b});

				for (int i = 0; i < 8; i++)
					ExtentMap[index++] = ba[i];
			}
		}

		public IEnumerable<ExtentPointer> GetAllocatedExtents()
		{
			int gamRangeStartPageID = (Header.Pointer.PageID / 511232) * 511232;
			
			for (int i = 0; i < ExtentMap.Length; i++)
				if (ExtentMap[i])
					yield return new ExtentPointer(new PagePointer(Header.Pointer.FileID, gamRangeStartPageID + i * 8));
		}
	}
}