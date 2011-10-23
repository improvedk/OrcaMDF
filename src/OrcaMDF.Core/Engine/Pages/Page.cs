using System;
using System.Linq;

namespace OrcaMDF.Core.Engine.Pages
{
	public class Page
	{
		public Database Database { get; private set; }
		public PageHeader Header;

		// Raw content of the page (8192 bytes)
		public byte[] RawBytes { get; private set; }

		public Page(byte[] bytes, Database database)
		{
			if (bytes.Length != 8192)
				throw new ArgumentException("bytes");

			Database = database;
			RawBytes = bytes;
			Header = new PageHeader(RawHeader);
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