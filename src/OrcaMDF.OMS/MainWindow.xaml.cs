using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.OMS
{
	public partial class MainWindow
	{
		public static RoutedCommand OpenDatabaseCommand = new RoutedCommand();
		
		private Database db;

		public MainWindow()
		{
			InitializeComponent();

			Unloaded += unloaded;
		}

		void unloaded(object sender, RoutedEventArgs e)
		{
			if (db != null)
				db.Dispose();
		}

		private void dumpException(Exception ex)
		{
			File.WriteAllText("ErrorLog.txt",
				DateTime.Now +
				Environment.NewLine +
				"----------" +
				Environment.NewLine +
				ex +
				Environment.NewLine);
		}

		private void file_exit(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void openDatabase(object sender, ExecutedRoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "SQL Server data files (*.mdf, *.ndf)|*.mdf;*.ndf";

			var result = dlg.ShowDialog();
			if (result == true)
			{
				try
				{
					var files = dlg.FileNames;
					db = new Database(files);

					refreshTreeview();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Please send the ErrorLog.txt file, from the application directory, to mark@improve.dk. Make sure to verify it does not contain any sensitive information before you send it.", "An error occurred while trying to open the database.", MessageBoxButton.OK, MessageBoxImage.Error);

					dumpException(ex);
				}
			}
		}

		private void refreshTreeview()
		{
			var rootNode = createNode(db.Name, null, null);

			// Add tables
			var tableRootNode = createNode("Tables", null, null);
			rootNode.Items.Add(tableRootNode);

			var tables = db.Dmvs.Tables.OrderBy(t => t.Name);

			foreach (var t in tables)
			{
				var tableNode = createNode(t.Name, "Table", null);
				tableRootNode.Items.Add(tableNode);

				// Add columns
				var tableColumnsNode = createNode("Columns", null, null);
				tableNode.Items.Add(tableColumnsNode);

				var columns = db.Dmvs.Columns
					.Where(c => c.ObjectID == t.ObjectID)
					.OrderBy(c => c.Name);

				foreach (var c in columns)
					tableColumnsNode.Items.Add(createNode(c.Name, "Column", null));

				// Add indexes
				var tableIndexesNode = createNode("Indexes", null, null);
				tableNode.Items.Add(tableIndexesNode);

				var indexes = db.Dmvs.Indexes
					.Where(i => i.ObjectID == t.ObjectID && i.IndexID > 0)
					.OrderBy(i => i.Name);

				foreach (var i in indexes)
					tableIndexesNode.Items.Add(createNode(i.Name, "Index", null));
			}

			// Refresh treeview
			treeview.Items.Clear();
			treeview.Items.Add(rootNode);
		}

		private TreeViewItem createNode(string text, string tag, string iconName)
		{
			var node = new TreeViewItem();
			node.Tag = tag;

			var panel = new StackPanel();
			
			if (iconName != null)
			{
				panel.Orientation = Orientation.Horizontal;
				
				using(var iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/OrcaMDF.OMS;component/Icons/" + iconName)).Stream)
				{
					var image = new Image();
					image.Height = 16;
					image.Source = BitmapFrame.Create(iconStream);
					panel.Children.Add(image);	
				}
			}

			if(tag == "Table")
				node.ContextMenu = treeview.Resources["TableContext"] as ContextMenu;

			panel.Children.Add(new TextBlock(new Run(text)));
			node.Header = panel;
			return node;
		}

		private void openDatabase_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void selectTop1000Rows(object sender, RoutedEventArgs e)
		{
			
		}
	}
}