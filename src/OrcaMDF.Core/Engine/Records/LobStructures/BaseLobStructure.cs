namespace OrcaMDF.Core.Engine.Records.LobStructures
{
	public abstract class BaseLobStructure
	{
		protected MdfFile File;

		protected BaseLobStructure(MdfFile file)
		{
			File = file;
		}
	}
}