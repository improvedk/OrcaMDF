namespace OrcaMDF.RawCore.Records
{
	public class RawIndexRecord : RawRecord
	{
		public RawIndexRecord(int index, RawPage page, RawDatabase database)
			: base (index, page, database)
		{ }
	}
}