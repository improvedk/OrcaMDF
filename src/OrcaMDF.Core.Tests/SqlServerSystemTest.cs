using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace OrcaMDF.Core.Tests
{
	[TestFixture]
	public abstract class SqlServerSystemTest
	{
		protected string DatabaseName = Guid.NewGuid().ToString();
		protected string[] DataFilePaths;
		protected string DataFileRootPath = ConfigurationManager.AppSettings["TestTempPath"];

		[TestFixtureSetUp]
		public void Setup()
		{
			// Create paths to datafiles
			var dataFiles = new List<string>();
			for (int i = 0; i < GetNumberOfFiles(); i++)
				dataFiles.Add(Path.Combine(DataFileRootPath, DatabaseName + "_" + i + "." + (i == 0 ? "mdf" : "ndf")));

			DataFilePaths = dataFiles.ToArray();

			// Create CREATE DATABASE statement
			string createStatement = @"
				CREATE DATABASE
					[<DBNAME>]
				ON PRIMARY ";

			// Add data files, trim trailing comma
			for(int i=0; i<DataFilePaths.Length; i++)
				createStatement += @"
					(
						NAME = N'<DBNAME>_" + i + @"',
						FILENAME = N'" + DataFilePaths[i] + @"',
						SIZE = 3MB,
						FILEGROWTH = 1MB
					),";
			createStatement = createStatement.Substring(0, createStatement.Length - 1);

			// Finish off CREATE statement
			createStatement += @"
				 LOG ON 
				(
					NAME = N'<DBNAME>_log',
					FILENAME = N'<TEMPPATH>\<DBNAME>_log.ldf',
					SIZE = 1MB,
					FILEGROWTH = 1MB
				)";

			// Connect to DB and CREATE database
			using(var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString))
			{
				conn.Open();

				var cmd = new SqlCommand(replaceDBParameters(createStatement), conn);

				cmd.ExecuteNonQuery();

				cmd.CommandText = replaceDBParameters("ALTER DATABASE [<DBNAME>] SET PAGE_VERIFY CHECKSUM");
				cmd.ExecuteNonQuery();

				using (var userConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString + ";Initial Catalog=" + DatabaseName))
				{
					userConn.Open();
					RunSetupQueries(userConn);

					new SqlCommand("USE master", userConn).ExecuteNonQuery();
				}

				cmd.CommandText = replaceDBParameters(@"
					ALTER DATABASE [<DBNAME>] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
					EXEC master.dbo.sp_detach_db @dbname = N'<DBNAME>'");
				cmd.ExecuteNonQuery();
			}
		}

		private string replaceDBParameters(string sql)
		{
			return sql.Replace("<DBNAME>", DatabaseName).Replace("<TEMPPATH>", ConfigurationManager.AppSettings["TestTempPath"]);
		}

		protected abstract void RunSetupQueries(SqlConnection conn);

		protected virtual short GetNumberOfFiles()
		{
			return 1;
		}

		protected void RunQuery(string sql, SqlConnection conn)
		{
			try
			{
				var cmd = new SqlCommand(sql, conn);
				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				Trace.WriteLine(ex);
				throw;
			}
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			File.Delete(Path.Combine(ConfigurationManager.AppSettings["TestTempPath"], DatabaseName + ".mdf"));
			File.Delete(Path.Combine(ConfigurationManager.AppSettings["TestTempPath"], DatabaseName + "_log.ldf"));
		}
	}
}