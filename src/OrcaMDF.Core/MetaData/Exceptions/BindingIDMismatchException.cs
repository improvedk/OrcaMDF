using System;

namespace OrcaMDF.Core.MetaData.Exceptions
{
	public sealed class BindingIDMismatchException : OrcaMDFException
	{
		public Guid BindingID { get; private set; }
		public Guid MismatchedBindingID { get; private set; }
		public string MismatchedDatabaseFile { get; private set; }

		public BindingIDMismatchException(string mismatchedDatabaseFile, Guid bindingID, Guid mismatchedBindingID) :
			base (string.Format("Database file {0} has a BindingID of {1} which does not match the sets BindingID of {2}", mismatchedDatabaseFile, bindingID, mismatchedBindingID))
		{
			MismatchedDatabaseFile = mismatchedDatabaseFile;
			BindingID = bindingID;
			MismatchedBindingID = mismatchedBindingID;
		}
	}
}