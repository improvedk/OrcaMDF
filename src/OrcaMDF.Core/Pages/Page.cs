using System;
using System.Linq;
using OrcaMDF.Core.Pages.PFS;

namespace OrcaMDF.Core.Pages
{
	public class Page
	{
		protected MdfFile File;

		// Raw content of the page (8192 bytes)
		public byte[] RawBytes { get; private set; }

		public PageHeader Header;

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
			return File.GetGamPage(GamPage.GetGamIndexByPageIndex(Header.PageID));
		}

		public SgamPage GetSgamPage()
		{
			return File.GetSgamPage(SgamPage.GetSgamIndexByPageIndex(Header.PageID));
		}

		public PfsPage GetPfsPage()
		{
			return File.GetPfsPage(PfsPage.GetPfsIndexByPageIndex(Header.PageID));
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
			return Header.ToString();
		}
	}
}