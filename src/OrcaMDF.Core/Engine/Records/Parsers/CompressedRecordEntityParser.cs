using System;
using System.Collections.Generic;
using OrcaMDF.Core.Engine.Pages;
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
			throw new NotImplementedException();
		}
	}
}