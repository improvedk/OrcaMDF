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
			File.AppendAllText("ErrorLog.txt",
				DateTime.Now +
				Environment.NewLine +
				"----------" +
				Environment.NewLine +
				ex +
				Environment.NewLine +
				Environment.NewLine);

			string msg =
				"An exception has occurred:" + Environment.NewLine +
				ex.Message + Environment.NewLine +
				Environment.NewLine +
				"To help improve OrcaMDF, I would appreciate if you would send the ErrorLog.txt file to me at mark@improve.dk" + Environment.NewLine +
				Environment.NewLine +
				"The error log does not contain any sensitive information, feel free to check it to be 100% certain. The ErrorLog.txt file is located in the same directory as the OrcaMDF Studio application.";

			MessageBox.Show(msg, "Uh oh!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

			// Add base tables
			addBaseTablesNode(rootNode);

			// Add DMVs
			addDmvNodes(rootNode);

			// Add tables
			addTablesNode(rootNode);

			// Add programmability
			addProgrammabilityNode(rootNode);
			

			// Refresh treeview
			treeview.Nodes.Clear();
			treeview.Nodes.Add(rootNode);
		}

		private void addProgrammabilityNode(TreeNode rootNode)
		{
			var prgRootNode = rootNode.Nodes.Add("Programmability");

			addStoredProceduresNode(prgRootNode);
			addViewsNode(prgRootNode);
		}

		private void addViewsNode(TreeNode prgRootNode)
		{
			var viewsNode = prgRootNode.Nodes.Add("Views");
			var views = db.Dmvs.Views.OrderBy(v => v.Name);

			foreach (var view in views)
			{
				var viewNode = viewsNode.Nodes.Add(view.Name);
				viewNode.ContextMenu = viewMenu;
			}
		}

		private void addStoredProceduresNode(TreeNode prgRootNode)
		{
			var proceduresNode = prgRootNode.Nodes.Add("Stored Procedures");
			var procedures = db.Dmvs.Procedures.OrderBy(p => p.Name);

			foreach (var proc in procedures)
			{
				var procNode = proceduresNode.Nodes.Add(proc.Name);
				procNode.ContextMenu = procedureMenu;
			}
		}

		private void addBaseTablesNode(TreeNode rootNode)
		{
			var baseTableNode = rootNode.Nodes.Add("Base Tables");
			baseTableNode.Nodes.Add("sys.sysallocunits").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.syscolpars").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysidxstats").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysiscols").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysobjvalues").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysowners").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysrowsets").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysrscols").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysscalartypes").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.sysschobjs").ContextMenu = baseTableMenu;
			baseTableNode.Nodes.Add("sys.syssingleobjrefs").ContextMenu = baseTableMenu;
		}

		private void addDmvNodes(TreeNode rootNode)
		{
			var dmvNode = rootNode.Nodes.Add("DMVs");
			dmvNode.Nodes.Add("sys.columns").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.database_principals").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.foreign_keys").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.indexes").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.index_columns").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.objects").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.objects$").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.partitions").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.procedures").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.sql_modules").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.system_internals_allocation_units").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.system_internals_partitions").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.system_internals_partition_columns").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.tables").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.types").ContextMenu = dmvMenu;
			dmvNode.Nodes.Add("sys.views").ContextMenu = dmvMenu;
		}

		private void addTablesNode(TreeNode rootNode)
		{
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

					foreach (var ic in indexColumns)
						indexNode.Nodes.Add(columns.Where(c => c.ColumnID == ic.ColumnID).Single().Name);
				}
			}
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

			gridStatusRows.Text = grid.Rows.Count + " Rows";
			txtCode.Visible = false;
			grid.Visible = true;
		}

		private void treeview_MouseUp(object sender, MouseEventArgs e)
		{
			// Make sure right clicking a node also selects it
			if (e.Button == MouseButtons.Right)
				treeview.SelectedNode = treeview.GetNodeAt(e.X, e.Y);
		}

		private IEnumerable<Row> getRowsFromDmv(string dmv)
		{
			switch (dmv)
			{
				case "sys.columns":
					return db.Dmvs.Columns.ToList();
				case "sys.database_principals":
					return db.Dmvs.DatabasePrincipals.ToList();
				case "sys.foreign_keys":
					return db.Dmvs.ForeignKeys.ToList();
				case "sys.indexes":
					return db.Dmvs.Indexes.ToList();
				case "sys.index_columns":
					return db.Dmvs.IndexColumns.ToList();
				case "sys.objects":
					return db.Dmvs.Objects.ToList();
				case "sys.objects$":
					return db.Dmvs.ObjectsDollar.ToList();
				case "sys.partitions":
					return db.Dmvs.Partitions.ToList();
				case "sys.procedures":
					return db.Dmvs.Procedures.ToList();
				case "sys.sql_modules":
					return db.Dmvs.SqlModules.ToList();
				case "sys.system_internals_allocation_units":
					return db.Dmvs.SystemInternalsAllocationUnits.ToList();
				case "sys.system_internals_partitions":
					return db.Dmvs.SystemInternalsPartitions.ToList();
				case "sys.system_internals_partition_columns":
					return db.Dmvs.SystemInternalsPartitionColumns.ToList();
				case "sys.tables":
					return db.Dmvs.Tables.ToList();
				case "sys.types":
					return db.Dmvs.Types.ToList();
				case "sys.views":
					return db.Dmvs.Views.ToList();
				default:
					throw new ArgumentOutOfRangeException(dmv);
			}
		}

		private void menuItem2_Click(object sender, EventArgs e)
		{
			showRows(getRowsFromDmv(treeview.SelectedNode.Text));
		}

		private IEnumerable<Row> getRowsFromBaseTable(string table)
		{
			switch(table)
			{
				case "sys.sysallocunits":
					return db.BaseTables.sysallocunits;
				case "sys.syscolpars":
					return db.BaseTables.syscolpars;
				case "sys.sysidxstats":
					return db.BaseTables.sysidxstats;
				case "sys.sysiscols":
					return db.BaseTables.sysiscols;
				case "sys.sysobjvalues":
					return db.BaseTables.sysobjvalues;
				case "sys.sysowners":
					return db.BaseTables.sysowners;
				case "sys.sysrowsets":
					return db.BaseTables.sysrowsets;
				case "sys.sysrscols":
					return db.BaseTables.sysrscols;
				case "sys.sysscalartypes":
					return db.BaseTables.sysscalartypes;
				case "sys.sysschobjs":
					return db.BaseTables.sysschobjs;
				case "sys.syssingleobjrefs":
					return db.BaseTables.syssingleobjrefs;
				default:
					throw new ArgumentOutOfRangeException(table);
			}
		}

		private void menuItem3_Click(object sender, EventArgs e)
		{
			showRows(getRowsFromBaseTable(treeview.SelectedNode.Text));
		}

		private void menuItem4_Click(object sender, EventArgs e)
		{
			showProcedureCode(treeview.SelectedNode.Text);
		}

		private void showProcedureCode(string procedureName)
		{
			// Get procedure ID
			int objID = db.Dmvs.Procedures
				.Where(p => p.Name == procedureName)
				.Select(p => p.ObjectID)
				.Single();

			// Get definition from sql_modules
			string definition = db.Dmvs.SqlModules
				.Where(m => m.ObjectID == objID)
				.Select(m => m.Definition)
				.Single();

			// Set code
			txtCode.Text = definition;

			grid.Visible = false;
			txtCode.Visible = true;
		}

		private void menuItem5_Click(object sender, EventArgs e)
		{
			showViewCode(treeview.SelectedNode.Text);
		}

		private void showViewCode(string viewName)
		{
			// Get view ID
			int objID = db.Dmvs.Views
				.Where(p => p.Name == viewName)
				.Select(p => p.ObjectID)
				.Single();

			// Get definition from sql_modules
			string definition = db.Dmvs.SqlModules
				.Where(m => m.ObjectID == objID)
				.Select(m => m.Definition)
				.Single();

			// Set code
			txtCode.Text = definition;

			grid.Visible = false;
			txtCode.Visible = true;
		}
	}
}