using System;
using System.Collections.Generic;
using System.IO;
using OrcaMDF.Core.Pages;
using OrcaMDF.Core.Pages.PFS;

namespace OrcaMDF.Core
{
	public class MdfFile : IDisposable
	{
		private readonly FileStream fs;
		private readonly IDictionary<int, byte[]> buffer = new Dictionary<int, byte[]>();

		public long NumberOfPages { get; private set; }

		public MdfFile(string path)
		{
			fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

			// Sanity check file length
			if (fs.Length % 8192 != 0)
				throw new ArgumentException("Invalid file length: " + fs.Length);

			NumberOfPages = fs.Length / 8192;
		}

		private byte[] getPageBytes(int index)
		{
			if(buffer.ContainsKey(index))
				return buffer[index];

			lock (buffer)
			{
				if (buffer.ContainsKey(index))
					return buffer[index];

				var bytes = new byte[8192];
				fs.Seek((long)index*8192, SeekOrigin.Begin);
				fs.Read(bytes, 0, 8192);
				buffer[index] = bytes;

				return bytes;
			}
		}

		public Page GetPage(int index)
		{
			return new Page(getPageBytes(index), this);
		}
		
		public TextMixPage GetTextMixPage(int index)
		{
			return new TextMixPage(getPageBytes(index), this);
		}

		public DataPage GetDataPage(int index)
		{
			return new DataPage(getPageBytes(index), this);
		}
		
		public IamPage GetIamPage(int index)
		{
			return new IamPage(getPageBytes(index), this);
		}

		public SgamPage GetSgamPage(int index)
		{
			if(index % 511230 != 3)
				throw new ArgumentException("Invalid SGAM index: " + index);

			return new SgamPage(getPageBytes(index), this);
		}

		public GamPage GetGamPage(int index)
		{
			if(index % 511230 != 2)
				throw new ArgumentException("Invalid GAM index: " + index);

			return new GamPage(getPageBytes(index), this);
		}

		public PfsPage GetPfsPage(int index)
		{
			// We know PFS pages are present every 8088th page, except for the very first one
			if(index != 1 && index % 8088 != 0)
				throw new ArgumentException("Invalid PFS index: " + index);

			return new PfsPage(getPageBytes(index), this);
		}

		public void Dispose()
		{
			fs.Dispose();
		}
	}
}