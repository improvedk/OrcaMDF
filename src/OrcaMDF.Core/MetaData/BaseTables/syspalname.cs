using System.Collections.Generic;

namespace OrcaMDF.Core.MetaData.BaseTables
{
	/// <summary>
	/// TODO
	/// This one's a bit ugly. Doesn't use Columns like all others. Can't be parsed as values are stored in resourceDB, which we can't parse.
	/// Hardcoded values instead - will refactor eventually, to be more consistent.
	/// </summary>
	internal class syspalname : Row
	{
		private static readonly ISchema schema = new Schema(new DataColumn[0]);

		internal string @class { get; set; }
		internal string value { get; set; }
		internal string name { get; set; }

		public syspalname() : base(schema)
		{ }

		public override Row NewRow()
		{
			return new syspalname();
		}

		internal static IList<syspalname> GetServer2008R2HardcodedValues()
		{
			return new[]
			{
			    new syspalname { @class = "AAST", value = "D", name = "DENY"},
			    new syspalname { @class = "AAST", value = "G", name = "GRANT"},
			    new syspalname { @class = "AULT", value = "AL", name = "APPLICATION LOG"},
			    new syspalname { @class = "AULT", value = "FL", name = "FILE"},
			    new syspalname { @class = "AULT", value = "SL", name = "SECURITY LOG"},
			    new syspalname { @class = "CEST", value = "CD", name = "CLOSED"},
			    new syspalname { @class = "CEST", value = "CO", name = "CONVERSING"},
			    new syspalname { @class = "CEST", value = "DI", name = "DISCONNECTED_INBOUND"},
			    new syspalname { @class = "CEST", value = "DO", name = "DISCONNECTED_OUTBOUND"},
			    new syspalname { @class = "CEST", value = "ER", name = "ERROR"},
			    new syspalname { @class = "CEST", value = "SI", name = "STARTED_INBOUND"},
			    new syspalname { @class = "CEST", value = "SO", name = "STARTED_OUTBOUND"},
			    new syspalname { @class = "CETY", value = "CP", name = "ENCRYPTED_BY_CRYPTOGRAPHIC_PROVIDER"},
			    new syspalname { @class = "CETY", value = "MK", name = "ENCRYPTED_BY_MASTER_KEY"},
			    new syspalname { @class = "CETY", value = "NA", name = "NO_PRIVATE_KEY"},
			    new syspalname { @class = "CETY", value = "PW", name = "ENCRYPTED_BY_PASSWORD"},
			    new syspalname { @class = "CETY", value = "SK", name = "ENCRYPTED_BY_SERVICE_MASTER_KEY"},
			    new syspalname { @class = "CRTY", value = "CPVA", name = "COUNTER SIGNATURE BY ASYMMETRIC KEY"},
			    new syspalname { @class = "CRTY", value = "CPVC", name = "COUNTER SIGNATURE BY CERTIFICATE"},
			    new syspalname { @class = "CRTY", value = "EPUA", name = "ENCRYPTION BY ASYMMETRIC KEY"},
			    new syspalname { @class = "CRTY", value = "EPUC", name = "ENCRYPTION BY CERTIFICATE"},
			    new syspalname { @class = "CRTY", value = "ESKM", name = "ENCRYPTION BY MASTER KEY"},
			    new syspalname { @class = "CRTY", value = "ESKP", name = "ENCRYPTION BY PASSWORD"},
			    new syspalname { @class = "CRTY", value = "ESKS", name = "ENCRYPTION BY SYMMETRIC KEY"},
			    new syspalname { @class = "CRTY", value = "INCP", name = "INDEPENDENT COMPONENT"},
			    new syspalname { @class = "CRTY", value = "SPVA", name = "SIGNATURE BY ASYMMETRIC KEY"},
			    new syspalname { @class = "CRTY", value = "SPVC", name = "SIGNATURE BY CERTIFICATE"},
			    new syspalname { @class = "DSTY", value = "FD", name = "FILESTREAM_DATA_FILEGROUP"},
			    new syspalname { @class = "DSTY", value = "FG", name = "ROWS_FILEGROUP"},
			    new syspalname { @class = "DSTY", value = "FL", name = "FILESTREAM_LOG_FILEGROUP"},
			    new syspalname { @class = "DSTY", value = "PS", name = "PARTITION_SCHEME"},
			    new syspalname { @class = "ECAL", value = "A1", name = "AES128"},
			    new syspalname { @class = "ECAL", value = "A2", name = "AES192"},
			    new syspalname { @class = "ECAL", value = "A3", name = "AES256"},
			    new syspalname { @class = "ECAL", value = "D", name = "DES"},
			    new syspalname { @class = "ECAL", value = "D3", name = "TRIPLE_DES"},
			    new syspalname { @class = "ECAL", value = "DT", name = "TRIPLE_DES_3KEY"},
			    new syspalname { @class = "ECAL", value = "DX", name = "DESX"},
			    new syspalname { @class = "ECAL", value = "R2", name = "RC2"},
			    new syspalname { @class = "ECAL", value = "R4", name = "RC4"},
			    new syspalname { @class = "ECAL", value = "RX", name = "RC4_128"},
			    new syspalname { @class = "ENAL", value = "1R", name = "RSA_512"},
			    new syspalname { @class = "ENAL", value = "2R", name = "RSA_1024"},
			    new syspalname { @class = "ENAL", value = "3R", name = "RSA_2048"},
			    new syspalname { @class = "ENAL", value = "A1", name = "AES_128"},
			    new syspalname { @class = "ENAL", value = "A2", name = "AES_192"},
			    new syspalname { @class = "ENAL", value = "A3", name = "AES_256"},
			    new syspalname { @class = "ENAL", value = "D ", name = "DES"},
			    new syspalname { @class = "ENAL", value = "D3", name = "TRIPLE_DES"},
			    new syspalname { @class = "ENAL", value = "DT", name = "TRIPLE_DES_3KEY"},
			    new syspalname { @class = "ENAL", value = "DX", name = "DESX"},
			    new syspalname { @class = "ENAL", value = "R2", name = "RC2"},
			    new syspalname { @class = "ENAL", value = "R4", name = "RC4"},
			    new syspalname { @class = "ENAL", value = "RX", name = "RC4_128"},
			    new syspalname { @class = "FTCT", value = "F", name = "FULL_CRAWL"},
			    new syspalname { @class = "FTCT", value = "I", name = "INCREMENTAL_CRAWL"},
			    new syspalname { @class = "FTCT", value = "P", name = "PAUSED_FULL_CRAWL"},
			    new syspalname { @class = "FTCT", value = "U", name = "UPDATE_CRAWL"},
			    new syspalname { @class = "HPRT", value = "BADB", name = "Backup Database"},
			    new syspalname { @class = "HPRT", value = "BALO", name = "Backup Transaction"},
			    new syspalname { @class = "HPRT", value = "CRDB", name = "Create Database"},
			    new syspalname { @class = "HPRT", value = "CRDF", name = "Create Default"},
			    new syspalname { @class = "HPRT", value = "CRFN", name = "Create Function"},
			    new syspalname { @class = "HPRT", value = "CRPR", name = "Create Procedure"},
			    new syspalname { @class = "HPRT", value = "CRRU", name = "Create Rule"},
			    new syspalname { @class = "HPRT", value = "CRTB", name = "Create Table"},
			    new syspalname { @class = "HPRT", value = "CRVW", name = "Create View"},
			    new syspalname { @class = "HPRT", value = "DL  ", name = "Delete"},
			    new syspalname { @class = "HPRT", value = "EX  ", name = "Execute"},
			    new syspalname { @class = "HPRT", value = "IN  ", name = "Insert"},
			    new syspalname { @class = "HPRT", value = "RF  ", name = "References"},
			    new syspalname { @class = "HPRT", value = "SL  ", name = "Select"},
			    new syspalname { @class = "HPRT", value = "UP  ", name = "Update"},
			    new syspalname { @class = "LGTY", value = "C", name = "CERTIFICATE_MAPPED_LOGIN"},
			    new syspalname { @class = "LGTY", value = "G", name = "WINDOWS_GROUP"},
			    new syspalname { @class = "LGTY", value = "K", name = "ASYMMETRIC_KEY_MAPPED_LOGIN"},
			    new syspalname { @class = "LGTY", value = "M", name = "COMPONENT_LOGIN"},
			    new syspalname { @class = "LGTY", value = "R", name = "SERVER_ROLE"},
			    new syspalname { @class = "LGTY", value = "S", name = "SQL_LOGIN"},
			    new syspalname { @class = "LGTY", value = "U", name = "WINDOWS_LOGIN"},
			    new syspalname { @class = "OBTY", value = "AF", name = "AGGREGATE_FUNCTION"},
			    new syspalname { @class = "OBTY", value = "C", name = "CHECK_CONSTRAINT"},
			    new syspalname { @class = "OBTY", value = "D", name = "DEFAULT_CONSTRAINT"},
			    new syspalname { @class = "OBTY", value = "EN", name = "EVENT_NOTIFICATION"},
			    new syspalname { @class = "OBTY", value = "F", name = "FOREIGN_KEY_CONSTRAINT"},
			    new syspalname { @class = "OBTY", value = "FN", name = "SQL_SCALAR_FUNCTION"},
			    new syspalname { @class = "OBTY", value = "FS", name = "CLR_SCALAR_FUNCTION"},
			    new syspalname { @class = "OBTY", value = "FT", name = "CLR_TABLE_VALUED_FUNCTION"},
			    new syspalname { @class = "OBTY", value = "IF", name = "SQL_INLINE_TABLE_VALUED_FUNCTION"},
			    new syspalname { @class = "OBTY", value = "IS", name = "SQL_INLINE_SCALAR_FUNCTION"},
			    new syspalname { @class = "OBTY", value = "IT", name = "INTERNAL_TABLE"},
			    new syspalname { @class = "OBTY", value = "P", name = "SQL_STORED_PROCEDURE"},
			    new syspalname { @class = "OBTY", value = "PC", name = "CLR_STORED_PROCEDURE"},
			    new syspalname { @class = "OBTY", value = "PK", name = "PRIMARY_KEY_CONSTRAINT"},
			    new syspalname { @class = "OBTY", value = "R", name = "RULE"},
			    new syspalname { @class = "OBTY", value = "RF", name = "REPLICATION_FILTER_PROCEDURE"},
			    new syspalname { @class = "OBTY", value = "S", name = "SYSTEM_TABLE"},
			    new syspalname { @class = "OBTY", value = "SN", name = "SYNONYM"},
			    new syspalname { @class = "OBTY", value = "SQ", name = "SERVICE_QUEUE"},
			    new syspalname { @class = "OBTY", value = "TA", name = "CLR_TRIGGER"},
			    new syspalname { @class = "OBTY", value = "TF", name = "SQL_TABLE_VALUED_FUNCTION"},
			    new syspalname { @class = "OBTY", value = "TR", name = "SQL_TRIGGER"},
			    new syspalname { @class = "OBTY", value = "TT", name = "TYPE_TABLE"},
			    new syspalname { @class = "OBTY", value = "U", name = "USER_TABLE"},
			    new syspalname { @class = "OBTY", value = "UQ", name = "UNIQUE_CONSTRAINT"},
			    new syspalname { @class = "OBTY", value = "V", name = "VIEW"},
			    new syspalname { @class = "OBTY", value = "X", name = "EXTENDED_STORED_PROCEDURE"},
			    new syspalname { @class = "PFTY", value = "C", name = "CLONE"},
			    new syspalname { @class = "PFTY", value = "H", name = "HASH"},
			    new syspalname { @class = "PFTY", value = "R", name = "RANGE"},
			    new syspalname { @class = "PRST", value = "D", name = "DENY"},
			    new syspalname { @class = "PRST", value = "G", name = "GRANT"},
			    new syspalname { @class = "PRST", value = "R", name = "REVOKE"},
			    new syspalname { @class = "PRST", value = "W", name = "GRANT_WITH_GRANT_OPTION"},
			    new syspalname { @class = "USTY", value = "A", name = "APPLICATION_ROLE"},
			    new syspalname { @class = "USTY", value = "C", name = "CERTIFICATE_MAPPED_USER"},
			    new syspalname { @class = "USTY", value = "G", name = "WINDOWS_GROUP"},
			    new syspalname { @class = "USTY", value = "K", name = "ASYMMETRIC_KEY_MAPPED_USER"},
			    new syspalname { @class = "USTY", value = "R", name = "DATABASE_ROLE"},
			    new syspalname { @class = "USTY", value = "S", name = "SQL_USER"},
			    new syspalname { @class = "USTY", value = "U", name = "WINDOWS_USER"},
			    new syspalname { @class = "XEER", value = "M", name = "ALLOW_MULTIPLE_EVENT_LOSS"},
			    new syspalname { @class = "XEER", value = "N", name = "NO_EVENT_LOSS"},
			    new syspalname { @class = "XEER", value = "S", name = "ALLOW_SINGLE_EVENT_LOSS"},
			    new syspalname { @class = "XEMP", value = "C", name = "PER_CPU"},
			    new syspalname { @class = "XEMP", value = "G", name = "NONE"},
			    new syspalname { @class = "XEMP", value = "N", name = "PER_NODE"}
			};
		}
	}
}