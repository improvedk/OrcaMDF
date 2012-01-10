using System;
using System.Text;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class BootPage : PrimaryRecordPage
	{
		public string DatabaseName { get; private set; }
		public PagePointer FirstSysIndexes { get; private set; }
		public short CreateVersion { get; private set; }
		public short Version { get; private set; }
		public int NextID { get; private set; }
		public long MaxDBTimeStamp { get; private set; }
		public int Status { get; private set; }
		public short DBID { get; private set; }

		public BootPage(byte[] bytes, Database database)
			: base(bytes, CompressionContext.NoCompression, database)
		{
			parseBootRecord();
		}

		private void parseBootRecord()
		{
			/*
				Bytes		Content
				-----		-------
				0-1			Version (smallint)
				2-3			CreateVersion (smallint)
				4-31		?
				32-35		Status (int)
				36-39		NextID (int)
				40-47		?
				48-303		DatabaseName (nchar(128))
				304-307		?
				308-309		DBID (smallint)
				310-311		?
				312-319		MaxDBTimeStamp (bigint)
				320-511		?
				512-515		FirstSysIndexes PageID (int)
				516-517		FirstSysIndexes FileID (smallint)
				518-1440	?
			*/

			byte[] bootRecord = Records[0].FixedLengthData;

			Version = BitConverter.ToInt16(bootRecord, 0);
			CreateVersion = BitConverter.ToInt16(bootRecord, 2);
			Status = BitConverter.ToInt32(bootRecord, 32);
			NextID = BitConverter.ToInt32(bootRecord, 36);
			
			// Truncate name at first 0x2020 char
			DatabaseName = Encoding.Unicode.GetString(bootRecord, 48, 256);
			if (DatabaseName.IndexOf('†') > 0)
				DatabaseName = DatabaseName.Substring(0, DatabaseName.IndexOf('†'));

			DBID = BitConverter.ToInt16(bootRecord, 308);
			MaxDBTimeStamp = BitConverter.ToInt64(bootRecord, 312);
			FirstSysIndexes = new PagePointer(BitConverter.ToInt16(bootRecord, 516), BitConverter.ToInt32(bootRecord, 512));
		}

		public override string ToString()
		{
			var sb = new StringBuilder(base.ToString());
			sb.AppendLine();

			sb.AppendLine("DatabaseName: " + DatabaseName);
			sb.AppendLine("FirstSysIndexes: " + FirstSysIndexes);
			sb.AppendLine("Version: " + Version);
			sb.AppendLine("CreateVersion: " + CreateVersion);
			sb.AppendLine("NextID: " + NextID);
			sb.AppendLine("MaxDBTimeStamp: " + MaxDBTimeStamp);
			sb.AppendLine("Status: " + Status);
			sb.AppendLine("DBID: " + DBID);

			return sb.ToString();
		}
	}
}