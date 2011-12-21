namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public abstract class LobStructureBase
	{
		protected Database Database;

		protected LobStructureBase(Database database)
		{
			Database = database;
		}
	}
}