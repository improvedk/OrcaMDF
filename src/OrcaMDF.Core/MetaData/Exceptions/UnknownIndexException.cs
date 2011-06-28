using System;

namespace OrcaMDF.Core.MetaData.Exceptions
{
	public class UnknownIndexException : Exception
	{
		public UnknownIndexException(string table, string index)
			: base("Unknown index '" + index + "' on table '" + table + "'")
		{ }
	}
}