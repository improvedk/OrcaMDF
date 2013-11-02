using System;

namespace OrcaMDF.Framework.SCSU.Exceptions
{
	public class IllegalInputException : Exception
	{
		public IllegalInputException()
			: base("The input character array or input byte array contained illegal sequences of bytes or characters.")
		{ }

		public IllegalInputException(string msg)
			: base(msg)
		{ }
	}
}