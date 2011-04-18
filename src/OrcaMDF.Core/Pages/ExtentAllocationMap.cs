using System.Collections;
using System.Linq;

namespace OrcaMDF.Core.Pages
{
	public class ExtentAllocationMap : Page
	{
		protected bool[] ExtentMap = new bool[63904];

		public ExtentAllocationMap(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseBody();
		}

		private void parseBody()
		{
			int index = 0;

			// Skip first 98 bytes and last 10
			foreach(byte b in RawBody.Skip(98).Take(7988))
			{
				var ba = new BitArray(new [] { b });

				for (int i = 0; i < 8; i++)
					ExtentMap[index++] = ba[i];
			}
		}
	}
}