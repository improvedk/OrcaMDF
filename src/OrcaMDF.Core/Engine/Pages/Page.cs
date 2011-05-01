using System;
using System.Linq;
using OrcaMDF.Core.Engine.Pages.PFS;

namespace OrcaMDF.Core.Engine.Pages
{
	public class Page
	{
		public MdfFile File { get; private set; }
		public PageHeader Header;

		// Raw content of the page (8192 bytes)
		public byte[] RawBytes { get; private set; }

		public Page(byte[] bytes, MdfFile file)
		{
			if (bytes.Length != 8192)
				throw new ArgumentException("bytes");

			File = file;
			RawBytes = bytes;
			Header = new PageHeader(RawHeader);
		}

		public GamPage GetGamPage()
		{
			return File.GetGamPage(GamPage.GetGamPointerForPage(Header.Pointer));
		}

		public SgamPage GetSgamPage()
		{
			return File.GetSgamPage(SgamPage.GetSgamPointerForPage(Header.Pointer));
		}

		public PfsPage GetPfsPage()
		{
			return File.GetPfsPage(PfsPage.GetPfsPointerForPage(Header.Pointer));
		}

		public byte[] RawHeader
		{
			get { return RawBytes.Take(96).ToArray(); }
		}

		public byte[] RawBody
		{
			get { return RawBytes.Skip(96).ToArray(); }
		}

		public override string  ToString()
		{
			return "{" + Header.Type + " (" + Header.Pointer.FileID + ":" + Header.Pointer.PageID + ")}";
		}
	}
}