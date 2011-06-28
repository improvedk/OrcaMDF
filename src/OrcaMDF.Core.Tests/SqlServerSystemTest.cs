using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using NUnit.Framework;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.Tests
{
	[TestFixture]
	public abstract class SqlServerSystemTest
	{
		private readonly string dbName = Guid.NewGuid().ToString();
		protected string MdfPath;

		[TestFixtureSetUp]
		public void Setup()
		{
			MdfPath = Path.Combine(ConfigurationManager.AppSettings["TestTempPath"], dbName + ".mdf");

			using(var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString))
			{
				conn.Open();

				var cmd = new SqlCommand(replaceDBParameters(@"
					CREATE DATABASE
						[<DBNAME>]
					ON PRIMARY 
						(
							NAME = N'<DBNAME>',
							FILENAME = N'<TEMPPATH>\<DBNAME>.mdf',
							SIZE = 3MB,
							FILEGROWTH = 1MB
						)
					LOG ON 
						(
							NAME = N'<DBNAME>_log',
							FILENAME = N'<TEMPPATH>\<DBNAME>_log.ldf',
							SIZE = 1MB,
							FILEGROWTH = 1MB
						)"), conn);
				cmd.ExecuteNonQuery();

				cmd.CommandText = replaceDBParameters("ALTER DATABASE [<DBNAME>] SET PAGE_VERIFY CHECKSUM");
				cmd.ExecuteNonQuery();

				using (var userConn = new SqlConnection(ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString + ";Initial Catalog=" + dbName))
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
			return sql.Replace("<DBNAME>", dbName).Replace("<TEMPPATH>", ConfigurationManager.AppSettings["TestTempPath"]);
		}

		protected abstract void RunSetupQueries(SqlConnection conn);

		[TestFixtureTearDown]
		public void TearDown()
		{
			File.Delete(Path.Combine(ConfigurationManager.AppSettings["TestTempPath"], dbName + ".mdf"));
			File.Delete(Path.Combine(ConfigurationManager.AppSettings["TestTempPath"], dbName + "_log.ldf"));
		}
	}
}