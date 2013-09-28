using System;

namespace OrcaMDF.Core.MetaData.Exceptions
{
	public abstract class OrcaMDFException : Exception
	{
		internal OrcaMDFException(string message) : base(message)
		{ }
	}
}