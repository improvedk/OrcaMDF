using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrcaMDF.Framework
{
	public static class Corruptor
	{
		/// <summary>
		/// Corrups an MDF file by overwriting pages with all zeros in random locations
		/// </summary>
		/// <param name="path">The path of the file to corrupt</param>
		/// <param name="corruptionPercentage">To percentage of the pages to corrupt. 0.1 = 10%</param>
		/// <returns>A list of the page IDs that were corrupted</returns>
		public static IEnumerable<int> CorruptFile(string path, double corruptionPercentage)
		{
			if (corruptionPercentage > 1)
				throw new ArgumentException("Corruption percentage can't be more than 100%");

			if (corruptionPercentage <= 0)
				throw new ArgumentException("Corruption percentage must be positive.");

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

		/// <summary>
		/// Corrups an MDF file by overwriting pages with all zeros in random locations
		/// </summary>
		/// <param name="path">The path of the file to corrupt</param>
		/// <param name="pagesToCorrupt">The number of pages to corrupt</param>
		/// <param name="startPageID">The inclusive lower bound page ID that may be corrupted</param>
		/// <param name="endPageID">The inclusive upper bound page ID that may be corrupted</param>
		/// <returns>A list of the page IDs that were corrupted</returns>
		public static IEnumerable<int> CorruptFile(string path, int pagesToCorrupt, int startPageID, int endPageID)
		{
			if (startPageID > endPageID)
				throw new ArgumentException("startPageID must be lower than or equal to endPageID.");

			if (pagesToCorrupt > (endPageID - startPageID + 1))
				throw new ArgumentException("Can't corrupt more pages than are available between startPageID and endPageID");

			using (var file = File.OpenWrite(path))
			{
				if (endPageID * 8192 + 8192 > file.Length)
					throw new ArgumentException("endPageID is larger than the amount of pages in the database file.");

				var rnd = new Random();
				byte[] zeros = new byte[8192];

				IEnumerable<int> pageIDsToCorrupt = Enumerable
					.Range(startPageID, (endPageID - startPageID + 1))
					.OrderBy(x => rnd.Next())
					.Take(pagesToCorrupt)
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