using System.Collections.Generic;
using OrcaMDF.Core.MetaData;

namespace OrcaMDF.Core.Engine.Records.Parsers
{
	internal abstract class RecordEntityParser
	{
		internal abstract IEnumerable<Row> GetEntities(Row schema);
	}
}