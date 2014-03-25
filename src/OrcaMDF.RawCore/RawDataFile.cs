using System;
using System.Collections.Generic;
using System.IO;

namespace OrcaMDF.RawCore
{
	public class RawDataFile : IDisposable
	{
		private readonly Stream stream;
		private readonly long fileSize;

		public int PageCount
		{
			get { return (int)(fileSize / 8192); }
		}

		public RawDataFile(string filePath)
		{
			stream = File.OpenRead(filePath);
			fileSize = new FileInfo(filePath).Length;
		}

		public RawPage GetPage(int pageID)
		{
			return new RawPage(pageID, GetPageBytes(pageID));
		}

		public byte[] GetPageBytes(int pageID)
		{
			stream.Seek(pageID * 8192, SeekOrigin.Begin);

			var bytes = new byte[8192];
			stream.Read(bytes, 0, 8192);

			return bytes;
		}

		/// <summary>
		/// Returns all the pages in the database that can be successfully parsed. Note that these may
		/// still be corrupt, but they're at least structurally valid.
		/// </summary>
		public IEnumerable<RawPage> BestEffortPages
		{
			get
			{
				for (int i = 0; i < PageCount; i++)
				{
					RawPage page = null;

					try
					{
						page = GetPage(i);
					}
					catch
					{ }

					if (page != null)
						yield return page;
				}
			}
		}

		/// <summary>
		/// Returns all the pages in the database
		/// </summary>
		public IEnumerable<RawPage> Pages
		{
			get
			{
				for (int i = 0; i < PageCount; i++)
					yield return GetPage(i);
			}
		}

		public void Dispose()
		{
			if (stream != null)
				stream.Dispose();
		}
	}
}