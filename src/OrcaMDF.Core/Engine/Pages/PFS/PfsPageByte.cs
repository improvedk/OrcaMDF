namespace OrcaMDF.Core.Engine.Pages.PFS
{
	public class PfsPageByte
	{
		public int PageID { get; private set; }
		public bool IsAllocated { get; private set; }
		public bool FromMixedExtent { get; private set; }
		public bool IsIAMPage { get; private set; }
		public bool ContainsGhostRecords { get; private set; }
		public byte Fullness { get; private set; }

		public PfsPageByte(byte data, int pageID)
		{
			this.PageID = pageID;

			parseData((PfsFlags)data);
		}

		private void parseData(PfsFlags data)
		{
			IsAllocated = (data & PfsFlags.Allocated) == PfsFlags.Allocated;
			FromMixedExtent = (data & PfsFlags.MixedExtent) == PfsFlags.MixedExtent;
			IsIAMPage = (data & PfsFlags.IAM) == PfsFlags.IAM;
			ContainsGhostRecords = (data & PfsFlags.GhostRecords) == PfsFlags.GhostRecords;

			if ((data & PfsFlags.UpTo50) == PfsFlags.UpTo50)
				Fullness = 50;
			else if ((data & PfsFlags.UpTo80) == PfsFlags.UpTo80)
				Fullness = 80;
			else if ((data & PfsFlags.UpTo95) == PfsFlags.UpTo95)
				Fullness = 95;
			else if ((data & PfsFlags.UpTo100) == PfsFlags.UpTo100)
				Fullness = 100;
		}
	}
}