namespace OrcaMDF.RawCore.Records
{
	public class RawIndexRecord : RawRecord
	{
		public RawIndexRecord(int index, RawPage page, RawDataFile dataFile)
			: base (index, page, dataFile)
		{ }
	}
}