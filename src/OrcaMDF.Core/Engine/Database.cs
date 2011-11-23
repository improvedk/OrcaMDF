using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OrcaMDF.Core.Engine.Pages;
using OrcaMDF.Core.Engine.Pages.PFS;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine
{
	public class Database : IDisposable
	{
		public string Name { get; private set; }
		public short ID { get; private set; }
		public DmvGenerator Dmvs { get; private set; }

		internal Dictionary<short, DataFile> Files = new Dictionary<short, DataFile>();
		internal BaseTableData BaseTables;
		internal Dictionary<string, object> ObjectCache = new Dictionary<string, object>();

		private readonly BufferManager bufferManager;
		private readonly object metaDataLock = new object();
		private DatabaseMetaData metaData;

		/// <summary>
		/// Instantiates a database using just a single data file.
		/// </summary>
		public Database(string file)
			: this(new [] { file })
		{ }

		/// <summary>
		/// Instantiates a new database. Each data file that's part of the database must be included in the files parameter.
		/// The order of the files is irrelevant.
		/// </summary>
		public Database(IEnumerable<string> files)
		{
			foreach(string file in files)
			{
				using(var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					byte[] fileHeaderBytes = new byte[8192];
					fs.Read(fileHeaderBytes, 0, 8192);

					// This is kindy hackish as "this" isn't operational yet. As such, any calls to "this" will fail.
					// We know however that, currently, FileHeaderPage doesn't use the reference. Should be changed
					// later on.
					var fileHeaderPage = new FileHeaderPage(fileHeaderBytes, this);

					// Store reference to data file
					Files.Add(fileHeaderPage.FileID, new DataFile(fileHeaderPage.FileID, file));
				}
			}

			// Instantiate buffer manager
			bufferManager = new BufferManager(this);

			// Read boot page properties
			var bootPage = GetBootPage();
			Name = bootPage.DatabaseName;
			ID = bootPage.DBID;

			// Parse vital base tables
			BaseTables = new BaseTableData(this);

			// Startup dmv generator
			Dmvs = new DmvGenerator(this);
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

			return new Page(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		[DebuggerStepThrough]
		public FileHeaderPage GetFileHeaderPage(PagePointer loc)
		{
			Debug.WriteLine("Loading File Header Page");

			return new FileHeaderPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		[DebuggerStepThrough]
		public NonclusteredIndexPage GetNonclusteredIndexPage(PagePointer loc)
		{
			Debug.WriteLine("Loading Nonclustered Index Page " + loc);

			return new NonclusteredIndexPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		[DebuggerStepThrough]
		public ClusteredIndexPage GetClusteredIndexPage(PagePointer loc)
		{
			Debug.WriteLine("Loading Clustered Index Page " + loc);

			return new ClusteredIndexPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		[DebuggerStepThrough]
		public TextMixPage GetTextMixPage(PagePointer loc)
		{
			Debug.WriteLine("Loading TextMix Page " + loc);

			return new TextMixPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		[DebuggerStepThrough]
		public DataPage GetDataPage(PagePointer loc)
		{
			Debug.WriteLine("Loading Data Page " + loc);

			return new DataPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		[DebuggerStepThrough]
		public IamPage GetIamPage(PagePointer loc)
		{
			Debug.WriteLine("Loading IAM Page " + loc);

			return new IamPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		[DebuggerStepThrough]
		public BootPage GetBootPage()
		{
			Debug.WriteLine("Loading Boot Page");

			return new BootPage(bufferManager.GetPageBytes(1, 9), this);
		}

		public SgamPage GetSgamPage(PagePointer loc)
		{
			Debug.WriteLine("Loading SGAM Page " + loc);

			if (loc.PageID % 511230 != 3)
				throw new ArgumentException("Invalid SGAM index: " + loc.PageID);

			return new SgamPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		public GamPage GetGamPage(PagePointer loc)
		{
			Debug.WriteLine("Loading GAM Page " + loc);

			if (loc.PageID % 511230 != 2)
				throw new ArgumentException("Invalid GAM index: " + loc.PageID);

			return new GamPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		public PfsPage GetPfsPage(PagePointer loc)
		{
			Debug.WriteLine("Loading PFS Page " + loc);

			// We know PFS pages are present every 8088th page, except for the very first one
			if (loc.PageID != 1 && loc.PageID % 8088 != 0)
				throw new ArgumentException("Invalid PFS index: " + loc.PageID);

			return new PfsPage(bufferManager.GetPageBytes(loc.FileID, loc.PageID), this);
		}

		public void Dispose()
		{
			bufferManager.Dispose();
		}
	}
}