namespace Orca.MdfReader.Adhoc
{
	public class PageLocation
	{
		public short FileID;
		public int PageID;

		public PageLocation(short fileID, int pageID)
		{
			this.FileID = fileID;
			this.PageID = pageID;
		}

		public override string ToString()
		{
			return "(" + FileID + ":" + PageID + ")";
		}
	}
}