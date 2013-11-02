using System;

namespace OrcaMDF.Framework.SCSU.Exceptions
{
	public class EndOfInputException : Exception
	{
		public EndOfInputException()
			: base("The input string or input byte array ended prematurely.")
		{ }

		public EndOfInputException(string msg)
			: base(msg)
		{ }
	}
}