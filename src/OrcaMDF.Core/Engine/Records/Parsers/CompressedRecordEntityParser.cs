using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;
using OrcaMDF.Core.Engine.Records.VariableLengthDataProxies;
using OrcaMDF.Core.Engine.SqlTypes;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.Records.Parsers
{
	internal class CompressedRecordEntityParser : RecordEntityParser
	{
		private readonly CompressedRecordPage page;

		internal CompressedRecordEntityParser(CompressedRecordPage page)
		{
			this.page = page;
		}

		internal override IEnumerable<Row> GetEntities(Row schema)
		{
			foreach (var record in page.Records)
			{
				var dataRow = schema.NewRow();
				var readState = new RecordReadState();

				int columnIndex = 0;
				foreach (DataColumn col in dataRow.Columns)
				{
					var sqlType = SqlTypeFactory.Create(col, readState, new CompressionContext(CompressionLevel.Row, true));

					IVariableLengthDataProxy dataProxy = record.GetPhysicalColumnBytes(columnIndex);

					if (dataProxy == null)
						dataRow[col] = null;
					else
						dataRow[col] = sqlType.GetValue(dataProxy.GetBytes().ToArray());

					columnIndex++;
				}

				yield return dataRow;
			}
		}

		internal override PagePointer NextPage
		{
			get { return page.Header.NextPage; }
		}
	}
}