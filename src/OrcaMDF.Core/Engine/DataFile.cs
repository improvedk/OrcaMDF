namespace OrcaMDF.Core.Engine
{
	public class DataFile
	{
		public short FileID { get; private set; }
		public string FilePath { get; private set; }

		public DataFile(short fileID, string path)
		{
			FileID = fileID;
			FilePath = path;
		}
	}
}