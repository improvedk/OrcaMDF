using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrcaMDF.Core.Engine.Pages.PFS
{
	public class PfsPage : Page
	{
		private IDictionary<int, PfsPageByte> pageDescriptions;

		public PfsPage(byte[] bytes, Database database)
			: base(bytes, database)
		{
			parseBody();
		}

		private void parseBody()
		{
			pageDescriptions = new Dictionary<int, PfsPageByte>();
			int pageID = Header.Pointer.PageID == 1 ? 0 : Header.Pointer.PageID;
			
			// Skip first 4
			// TODO: Should treat this as the record it is, instead of misusing RawBody
			foreach(byte pageByte in RawBody.Skip(4).Take(8088))
			{
				var pfsPageDescription = new PfsPageByte(pageByte, pageID);
				pageDescriptions.Add(pageID++, pfsPageDescription);
			}
		}

		public PfsPageByte GetPageDescription(int pageID)
		{
			if (!pageDescriptions.ContainsKey(pageID))
				throw new ArgumentException("PageID is not tracked by this PFS page: " + pageID);

			return pageDescriptions[pageID];
		}

		public static PagePointer GetPfsPointerForPage(PagePointer loc)
		{
			// First pfs page is at index 1 and every 8088 pages hereafter
			return new PagePointer(loc.FileID, Math.Max(loc.PageID / 8088 * 8088, 1));
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("PageID\tStatus");

			foreach(var dsc in pageDescriptions.Values)
				sb.AppendLine(dsc.PageID + "\t" + (dsc.IsAllocated ? "ALLOCATED\t" : "NOT ALLOCATED\t") + (dsc.Fullness + "\t") + (dsc.IsIAMPage ? "IAM\t" : "\t") + (dsc.FromMixedExtent ? "MIXED EXT\t" : "\t\t") + (dsc.ContainsGhostRecords ? "GHOSTS\t" : "\t"));

			return sb.ToString();
		}
	}
}