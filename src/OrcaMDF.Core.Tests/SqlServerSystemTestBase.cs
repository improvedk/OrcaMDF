using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OrcaMDF.Core.Engine;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests
{
	[TestFixture]
	public abstract class SqlServerSystemTestBase
	{
		private readonly Dictionary<DatabaseVersion, string[]> databaseFiles = new Dictionary<DatabaseVersion,string[]>();
		private readonly string databaseBaseName = Guid.NewGuid().ToString();

		protected string DataFileRootPath = ConfigurationManager.AppSettings["TestTempPath"];

		protected void RunDatabaseTest(DatabaseVersion version, Action<Database> test)
		{
			Debug.WriteLine("Test database: " + databaseBaseName);

			// Setup database and store file paths, if we haven't done so already
			ensureDatabaseIsSetup(version);

			// Run actual test
			using (var db = new Database(databaseFiles[version]))
				test(db);
		}

		private void ensureDatabaseIsSetup(DatabaseVersion version)
		{
			if (databaseFiles.ContainsKey(version))
				return;

			string dbName = databaseBaseName + "_" + version;
			string connectionString = ConfigurationManager.ConnectionStrings[version.ToString()].ConnectionString;

			// Create paths to datafiles
			var dataFiles = new string[GetNumberOfFiles()];
			for (int i = 0; i < GetNumberOfFiles(); i++)
				dataFiles[i] = Path.Combine(DataFileRootPath, dbName + "_" + i + "." + (i == 0 ? "mdf" : "ndf"));

			databaseFiles[version] = dataFiles;

			// Create CREATE DATABASE statement
			string createStatement = @"
				CREATE DATABASE
					[<DBNAME>]
				ON PRIMARY ";

			// Add data files, trim trailing comma
			for (int i = 0; i < dataFiles.Length; i++)
				createStatement += @"
					(
						NAME = N'<DBNAME>_" + i + @"',
						FILENAME = N'" + dataFiles[i] + @"',
						SIZE = 3MB,
						FILEGROWTH = 1MB
					),";
			createStatement = createStatement.Substring(0, createStatement.Length - 1);

			// Finish off CREATE statement
			createStatement += @"
				 LOG ON 
				(
					NAME = N'<DBNAME>_log',
					FILENAME = N'<TEMPPATH>\<DBNAME>.ldf',
					SIZE = 1MB,
					FILEGROWTH = 1MB
				)";

			// Connect to DB and CREATE database
			using (var conn = new SqlConnection(connectionString))
			{
				conn.Open();

				var cmd = new SqlCommand(replaceDBParameters(createStatement, dbName), conn);

				cmd.ExecuteNonQuery();

				cmd.CommandText = replaceDBParameters("ALTER DATABASE [<DBNAME>] SET PAGE_VERIFY CHECKSUM", dbName);
				cmd.ExecuteNonQuery();

				try
				{
					using (var userConn = new SqlConnection(connectionString + ";Initial Catalog=" + dbName))
					{
						userConn.Open();

						try
						{
							RunSetupQueries(userConn, version);
						}
						finally
						{
							new SqlCommand("USE master", userConn).ExecuteNonQuery();
						}
					}
				}
				finally
				{
					cmd.CommandText = replaceDBParameters(@"
						ALTER DATABASE [<DBNAME>] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
						EXEC master.dbo.sp_detach_db @dbname = N'<DBNAME>'", dbName);
					cmd.ExecuteNonQuery();
				}
			}
		}

		private string replaceDBParameters(string sql, string dbName)
		{
			return sql.Replace("<DBNAME>", dbName).Replace("<TEMPPATH>", DataFileRootPath);
		}

		protected abstract void RunSetupQueries(SqlConnection conn, DatabaseVersion version);

		protected virtual short GetNumberOfFiles()
		{
			return 1;
		}

		protected void RunQuery(string sql, SqlConnection conn)
		{
			var cmd = new SqlCommand(sql, conn);
			cmd.CommandTimeout = 600;
			cmd.ExecuteNonQuery();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			// Delete all collateral files resulting from running this test
			foreach(var version in databaseFiles.Keys)
			{
				var files = databaseFiles[version];

				// Delete log file
				File.Delete(files[0].Replace("_0.mdf", ".ldf"));

				// Delete data file(s)
				foreach(var file in files)
					File.Delete(file);
			}
		}
	}
}