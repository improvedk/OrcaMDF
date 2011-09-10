using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OrcaMDF.Core.Engine.Pages;
using OrcaMDF.Core.Engine.Pages.PFS;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine
{
	public class MdfFile : IDisposable
	{
		private readonly FileStream fs;
		private readonly IDictionary<int, byte[]> buffer = new Dictionary<int, byte[]>();
		private object metaDataLock = new object();
		private DatabaseMetaData metaData;

		public long NumberOfPages { get; private set; }
		public long NumberOfExtents { get; private set; }

		public MdfFile(string path)
		{
			fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

			// Sanity check file length
			if (fs.Length % 8192 != 0)
				throw new ArgumentException("Invalid file length: " + fs.Length);

			NumberOfPages = fs.Length / 8192;
			NumberOfExtents = NumberOfPages / 8;
		}

		[DebuggerStepThrough]
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

		public DatabaseMetaData GetMetaData()
		{
			if (metaData == null)
			{
				lock (metaDataLock)
				{
					if (metaData == null)
						metaData = new DatabaseMetaData(this);
				}
			}

			return metaData;
		}

		[DebuggerStepThrough]
		public Page GetPage(PagePointer loc)
		{
			Debug.WriteLine("Loading Generic Page " + loc);

			// TODO: Ensure loc.FileID matches

			return new Page(getPageBytes(loc.PageID), this);
		}

		[DebuggerStepThrough]
		public NonclusteredIndexPage GetNonclusteredIndexPage(PagePointer loc)
		{
			Debug.WriteLine("Loading Nonclustered Index Page " + loc);

			// TODO: Ensure loc.FileID matches

			return new NonclusteredIndexPage(getPageBytes(loc.PageID), this);
		}

		[DebuggerStepThrough]
		public ClusteredIndexPage GetClusteredIndexPage(PagePointer loc)
		{
			Debug.WriteLine("Loading Clustered Index Page " + loc);

			// TODO: Ensure loc.FileID matches

			return new ClusteredIndexPage(getPageBytes(loc.PageID), this);
		}

		[DebuggerStepThrough]
		public TextMixPage GetTextMixPage(PagePointer loc)
		{
			Debug.WriteLine("Loading TextMix Page " + loc);

			// TODO: Ensure loc.FileID matches

			return new TextMixPage(getPageBytes(loc.PageID), this);
		}

		[DebuggerStepThrough]
		public DataPage GetDataPage(PagePointer loc)
		{
			Debug.WriteLine("Loading Data Page " + loc);

			// TODO: Ensure loc.FileID matches

			return new DataPage(getPageBytes(loc.PageID), this);
		}

		[DebuggerStepThrough]
		public IamPage GetIamPage(PagePointer loc)
		{
			Debug.WriteLine("Loading IAM Page " + loc);

			// TODO: Ensure loc.FileID matches

			return new IamPage(getPageBytes(loc.PageID), this);
		}

		[DebuggerStepThrough]
		public BootPage GetBootPage()
		{
			Debug.WriteLine("Loading Boot Page");

			// TODO: Assert this file is file 1 in the PRIMARY filegroup

			return new BootPage(getPageBytes(9), this);
		}

		public SgamPage GetSgamPage(PagePointer loc)
		{
			Debug.WriteLine("Loading SGAM Page " + loc);

			// TODO: Ensure loc.FileID matches

			if(loc.PageID % 511230 != 3)
				throw new ArgumentException("Invalid SGAM index: " + loc.PageID);

			return new SgamPage(getPageBytes(loc.PageID), this);
		}

		public GamPage GetGamPage(PagePointer loc)
		{
			Debug.WriteLine("Loading GAM Page " + loc);

			// TODO: Ensure loc.FileID matches

			if(loc.PageID % 511230 != 2)
				throw new ArgumentException("Invalid GAM index: " + loc.PageID);

			return new GamPage(getPageBytes(loc.PageID), this);
		}

		public PfsPage GetPfsPage(PagePointer loc)
		{
			Debug.WriteLine("Loading PFS Page " + loc);

			// TODO: Ensure loc.FileID matches

			// We know PFS pages are present every 8088th page, except for the very first one
			if(loc.PageID != 1 && loc.PageID % 8088 != 0)
				throw new ArgumentException("Invalid PFS index: " + loc.PageID);

			return new PfsPage(getPageBytes(loc.PageID), this);
		}

		public void Dispose()
		{
			fs.Dispose();
		}
	}
}