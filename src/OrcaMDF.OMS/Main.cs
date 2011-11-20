using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.MetaData;

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

			// Add DMVs
			var dmvNode = rootNode.Nodes.Add("DMVs");
			dmvNode.Nodes.Add("sys.columns").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.foreign_keys").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.indexes").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.index_columns").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.objects").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.objects$").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.system_internals_allocation_units").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.system_internals_partitions").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.system_internals_partition_columns").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.tables").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.types").ContextMenu = dmvMenu;

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
				showRows(rows);
			}
			catch (Exception ex)
			{
				logException(ex);
			}
		}

		private void showRows(IEnumerable<Row> rows)
		{
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

		private void treeview_MouseUp(object sender, MouseEventArgs e)
		{
			// Make sure right clicking a node also selects it
			if (e.Button == MouseButtons.Right)
				treeview.SelectedNode = treeview.GetNodeAt(e.X, e.Y);
		}

		private void menuItem2_Click(object sender, EventArgs e)
		{
			switch(treeview.SelectedNode.Text)
			{
				case "sys.columns":
					showRows(db.Dmvs.Columns.ToList());
					break;

				case "sys.foreign_keys":
					showRows(db.Dmvs.ForeignKeys.ToList());
					break;

				case "sys.indexes":
					showRows(db.Dmvs.Indexes.ToList());
					break;

				case "sys.index_columns":
					showRows(db.Dmvs.IndexColumns.ToList());
					break;

				case "sys.objects":
					showRows(db.Dmvs.Objects.ToList());
					break;

				case "sys.objects$":
					showRows(db.Dmvs.ObjectsDollar.ToList());
					break;

				case "sys.system_internals_allocation_units":
					showRows(db.Dmvs.SystemInternalsAllocationUnits.ToList());
					break;

				case "sys.system_internals_partitions":
					showRows(db.Dmvs.SystemInternalsPartitions.ToList());
					break;

				case "sys.system_internals_partition_columns":
					showRows(db.Dmvs.SystemInternalsPartitionColumns.ToList());
					break;

				case "sys.tables":
					showRows(db.Dmvs.Tables.ToList());
					break;

				case "sys.types":
					showRows(db.Dmvs.Types.ToList());
					break;
			}
		}
	}
}