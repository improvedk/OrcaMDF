using System;

namespace OrcaMDF.Core.MetaData.Exceptions
{
	public class UnknownTableException : Exception
	{
		public UnknownTableException(string table)
			: base("Unknown table '" + table + "'")
		{ }
	}
}