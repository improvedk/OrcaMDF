using System.Collections.Generic;
using System.IO;

namespace OrcaMDF.RawCore
{
	public class RawDatabase
	{
		internal Dictionary<short, byte[]> Data;

		public RawDatabase(string filePath)
		{
			Data = new Dictionary<short, byte[]>();
			Data.Add(1, File.ReadAllBytes(filePath));
		}

		public RawPage GetPage(short fileID, int pageID)
		{
			return new RawPage(fileID, pageID, this);
		}

		public IEnumerable<RawPage> GetAllPages(short fileID)
		{
			int numberOfPages = (int)(Data[fileID].LongLength / 8192);

			for (int i = 0; i < numberOfPages; i++)
				yield return GetPage(fileID, i);
		}
	}
}