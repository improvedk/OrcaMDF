namespace OrcaMDF.Core.MetaData.Exceptions
{
	public class UnknownTableException : OrcaMDFException
	{
		public UnknownTableException(string table)
			: base("Unknown table '" + table + "'")
		{ }
	}
}