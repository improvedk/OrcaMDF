using System;
using System.Collections.Generic;
using System.IO;

namespace OrcaMDF.RawCore
{
	public class RawDatabase : IDisposable
	{
		//internal Dictionary<short, byte[]> Data;
		private Dictionary<short, Stream> fileStreams = new Dictionary<short,Stream>();
		private Dictionary<short, long> fileSizes = new Dictionary<short,long>();

		public RawDatabase(string filePath)
		{
			//Data = new Dictionary<short, byte[]>();
			//Data.Add(1, File.ReadAllBytes(filePath));
			fileStreams.Add(1, File.OpenRead(filePath));
			fileSizes.Add(1, new FileInfo(filePath).Length);
		}

		public RawPage GetPage(short fileID, int pageID)
		{
			return new RawPage(fileID, pageID, this);
		}

		public byte[] GetPageBytes(short fileID, int pageID)
		{
			var stream = fileStreams[fileID];
			stream.Seek(pageID * 8192, SeekOrigin.Begin);

			var bytes = new byte[8192];
			stream.Read(bytes, 0, 8192);

			return bytes;
		}

		public IEnumerable<RawPage> Pages
		{
			get
			{
				foreach (var fileID in fileStreams.Keys)
				{
					int numberOfPages = (int)(fileSizes[fileID] / 8192);

					for (int i = 0; i < numberOfPages; i++)
						yield return GetPage(fileID, i);
				}
			}
		}

		public void Dispose()
		{
			foreach (var stream in fileStreams.Values)
				stream.Dispose();
		}
	}
}