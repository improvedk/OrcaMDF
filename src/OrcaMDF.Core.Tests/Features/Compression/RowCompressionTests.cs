using System.Data.SqlClient;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Compression
{
	public class RowCompressionTests : SqlServerSystemTestBase
	{
		protected override void RunSetupQueries(SqlConnection conn, DatabaseVersion version)
		{
			RunQuery(@"
				
			", conn);
		}
	}
}