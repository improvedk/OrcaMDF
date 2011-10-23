using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine
{
	public abstract class Scanner
	{
		protected readonly Database Database;

		// Lazy loading metadata as some scan operations don't need metadata - and we use those to scan the metadata
		// structures themselves.
		private DatabaseMetaData metaData;
		protected DatabaseMetaData MetaData
		{
			get
			{
				if (metaData == null)
					metaData = Database.GetMetaData();
				
				return metaData;
			}
		}

		protected Scanner(Database database)
		{
			Database = database;
		}
	}
}