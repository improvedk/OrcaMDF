namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public abstract class BaseLobStructure
	{
		protected Database Database;

		protected BaseLobStructure(Database database)
		{
			Database = database;
		}
	}
}