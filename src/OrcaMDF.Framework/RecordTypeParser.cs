namespace OrcaMDF.Framework
{
	public static class RecordTypeParser
	{
		public static RecordType Parse(byte statusByteA)
		{
			return (RecordType)((statusByteA & 0x0E) >> 1);
		}
	}
}