using System;
using System.Data;
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

		private void logException(Exception ex)
		{
			File.WriteAllText("ErrorLog.txt",
				DateTime.Now +
				Environment.NewLine +
				"----------" +
				Environment.NewLine +
				ex +
				Environment.NewLine);

			MessageBox.Show("Please send the ErrorLog.txt file, from the application directory, to mark@improve.dk. Make sure to verify it does not contain any sensitive information before you send it.", "An error occurred while trying to open the database.", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					logException(ex);
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
				tableNode.ContextMenu = tableMenu;

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
				{
					var indexNode = tableIndexesNode.Nodes.Add(i.Name);

					// Add index columns
					var indexColumns = db.Dmvs.IndexColumns
						.Where(ic => ic.ObjectID == t.ObjectID && ic.IndexID == i.IndexID);

					foreach(var ic in indexColumns)
						indexNode.Nodes.Add(columns.Where(c => c.ColumnID == ic.ColumnID).Single().Name);
				}
			}

			// Refresh treeview
			treeview.Nodes.Clear();
			treeview.Nodes.Add(rootNode);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void menuItem1_Click(object sender, EventArgs e)
		{
			loadTable(treeview.SelectedNode.Text);
		}

		private void loadTable(string table)
		{
			try
			{
				var scanner = new DataScanner(db);
				var rows = scanner.ScanTable(table).Take(1000);

				grid.DataSource = null;

				if (rows.Count() > 0)
				{
					var ds = new DataSet();
					var tbl = new DataTable();
					ds.Tables.Add(tbl);

					var firstRow = rows.First();

					foreach (var col in firstRow.Columns)
						tbl.Columns.Add(col.Name);

					foreach (var scannedRow in rows)
					{
						var row = tbl.NewRow();

						foreach (var col in scannedRow.Columns)
							row[col.Name] = scannedRow[col];

						tbl.Rows.Add(row);
					}

					grid.DataSource = tbl;
				}
			}
			catch (Exception ex)
			{
				logException(ex);
			}
		}

		private void treeview_MouseUp(object sender, MouseEventArgs e)
		{
			// Make sure right clicking a node also selects it
			if (e.Button == MouseButtons.Right)
				treeview.SelectedNode = treeview.GetNodeAt(e.X, e.Y);
		}
	}
}