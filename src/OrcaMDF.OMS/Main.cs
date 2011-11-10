using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.OMS
{
	public partial class Main : Form
	{
		private Database db;

		public Main()
		{
			InitializeComponent();

			Disposed += Main_Disposed;
		}

		void Main_Disposed(object sender, EventArgs e)
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

		private void openToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			var result = openDatabaseDialog.ShowDialog();

			if(result == DialogResult.OK)
			{
				try
				{
					var files = openDatabaseDialog.FileNames;
					db = new Database(files);

					refreshTreeview();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Please send the ErrorLog.txt file, from the application directory, to mark@improve.dk. Make sure to verify it does not contain any sensitive information before you send it.", "An error occurred while trying to open the database.", MessageBoxButtons.OK, MessageBoxIcon.Error);

					dumpException(ex);
				}
			}
		}

		private void refreshTreeview()
		{
			var rootNode = new TreeNode(db.Name);

			// Add tables
			var tableRootNode = rootNode.Nodes.Add("Tables");
			var tables = db.Dmvs.Tables.OrderBy(t => t.Name);

			foreach (var t in tables)
			{
				var tableNode = tableRootNode.Nodes.Add(t.Name);

				// Add columns
				var tableColumnsNode = tableNode.Nodes.Add("Columns");
				var columns = db.Dmvs.Columns
					.Where(c => c.ObjectID == t.ObjectID)
					.OrderBy(c => c.Name);

				foreach (var c in columns)
					tableColumnsNode.Nodes.Add(c.Name);

				// Add indexes
				var tableIndexesNode = tableNode.Nodes.Add("Indexes");
				var indexes = db.Dmvs.Indexes
					.Where(i => i.ObjectID == t.ObjectID && i.IndexID > 0)
					.OrderBy(i => i.Name);

				foreach (var i in indexes)
					tableIndexesNode.Nodes.Add(i.Name);
			}

			// Refresh treeview
			treeview.Nodes.Clear();
			treeview.Nodes.Add(rootNode);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}