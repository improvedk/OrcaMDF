namespace OrcaMDF.Core.MetaData.Exceptions
{
	public class UnknownIndexException : OrcaMDFException
	{
		public UnknownIndexException(string table, string index)
			: base("Unknown index '" + index + "' on table '" + table + "'")
		{ }
	}
}