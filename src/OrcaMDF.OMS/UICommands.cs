using System.Windows.Input;

namespace OrcaMDF.OMS
{
	public static class UICommands
	{
		public static RoutedUICommand OpenDatabase { get; private set; }

		static UICommands()
		{
			OpenDatabase = new RoutedUICommand("OpenDatabase", "OpenDatabase", typeof(UICommands));
		}
	}
}