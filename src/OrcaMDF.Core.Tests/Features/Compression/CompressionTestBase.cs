using NUnit.Framework;
using OrcaMDF.Core.Tests.SqlServerVersion;

namespace OrcaMDF.Core.Tests.Features.Compression
{
    public abstract class CompressionTestBase : SqlServerSystemTestBase
    {
        [TestFixtureSetUp]
        public void CompressionSetup()
        {
            if (!SupportsCompression(DatabaseVersion.SqlServer2008R2)) {Assert.Ignore("This Sql Server instance does not suport compression");}
        }
    }
}