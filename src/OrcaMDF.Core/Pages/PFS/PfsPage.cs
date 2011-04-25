using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrcaMDF.Core.Pages.PFS
{
	public class PfsPage : Page
	{
		private IDictionary<int, PfsPageByte> pageDescriptions;

		public PfsPage(byte[] bytes, MdfFile file)
			: base(bytes, file)
		{
			parseBody();
		}

		private void parseBody()
		{
			pageDescriptions = new Dictionary<int, PfsPageByte>();
			int pageID = Header.PageID == 1 ? 0 : Header.PageID;
			
			// Skip first 4 & last 4 bytes
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

		public static int GetPfsIndexByPageIndex(int index)
		{
			// First pfs page is at index 1 and every 8088 pages hereafter
			return Math.Max(index / 8088 * 8088, 1);
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