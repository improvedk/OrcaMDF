using System;
using System.Linq;
using System.Text;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class FileHeaderPage : PrimaryRecordPage
	{
		public Guid BindingID { get; private set; }
		public short FileID { get; private set; }

		public FileHeaderPage(byte[] bytes, Database database)
			: base(bytes, CompressionContext.NoCompression, database)
		{
			parseFileHeaderRecord();
		}

		private void parseFileHeaderRecord()
		{
			var record = this.Records[0];

			/*
				Varlength field index		Content
				-----						-------
				0							BindingID (guid)
				1							?
				2							FileIdProp (short)
				3-46						?
			*/

			BindingID = new Guid(record.VariableLengthColumnData[0].GetBytes().ToArray());
			FileID = BitConverter.ToInt16(record.VariableLengthColumnData[2].GetBytes().ToArray(), 0);
		}

		public override string ToString()
		{
			var sb = new StringBuilder(base.ToString());
			sb.AppendLine();

			sb.AppendLine("BindingID: " + BindingID);
			sb.AppendLine("FileID: " + FileID);
			
			return sb.ToString();
		}
	}
}