using System;

namespace OrcaMDF.Core.MetaData.Exceptions
{
	public sealed class BindingIdMismatchException : OrcaMDFException
	{
		public Guid BindingId { get; private set; }
		public Guid MismatchedBindingId { get; private set; }
		public string MismatchedDatabaseFile { get; private set; }

		public BindingIdMismatchException(string mismatchedDatabaseFile, Guid bindingId, Guid mismatchedBindingId) :
			base (string.Format("Database file {0} has a BindingId of {1} which does not match the sets BindingId of {2}", mismatchedDatabaseFile, bindingId, mismatchedBindingId))
		{
			MismatchedDatabaseFile = mismatchedDatabaseFile;
			BindingId = bindingId;
			MismatchedBindingId = mismatchedBindingId;
		}
	}
}