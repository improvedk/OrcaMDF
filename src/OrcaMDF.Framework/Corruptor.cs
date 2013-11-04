using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrcaMDF.Framework
{
	public static class Corruptor
	{
		public static IEnumerable<int> CorruptFile(string path, double corruptionPercentage)
		{
			using (var file = File.OpenWrite(path))
			{
				var rnd = new Random();
				byte[] zeros = new byte[8192];

				int pageCount = (int)(file.Length / 8192);
				int pageCountToCorrupt = (int)(pageCount * corruptionPercentage);
				
				IEnumerable<int> pageIDsToCorrupt = Enumerable
					.Range(0, pageCount)
					.OrderBy(x => rnd.Next())
					.Take(pageCountToCorrupt)
					.ToList();

				foreach (int pageID in pageIDsToCorrupt)
				{
					file.Position = pageID * 8192;
					file.Write(zeros, 0, 8192);
				}

				return pageIDsToCorrupt;
			}
		}
	}
}