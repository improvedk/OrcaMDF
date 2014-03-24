using System;
using System.Collections.Generic;
using System.IO;

namespace OrcaMDF.RawCore
{
	public class RawDataFile : IDisposable
	{
		private readonly Stream stream;
		private readonly long fileSize;

		public long PageCount
		{
			get { return fileSize / 8192; }
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

		public IEnumerable<RawPage> Pages
		{
			get
			{
				int numberOfPages = (int)(fileSize / 8192);

				for (int i = 0; i < numberOfPages; i++)
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