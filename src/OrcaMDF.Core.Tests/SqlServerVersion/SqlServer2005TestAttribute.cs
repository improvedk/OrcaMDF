using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;

namespace OrcaMDF.Core.Tests.SqlServerVersion
{
	public class SqlServer2005TestAttribute : TestCaseSourceAttribute
	{
		private static IEnumerable<TestCaseData> versions
		{
			get
			{
				if (ConfigurationManager.ConnectionStrings[DatabaseVersion.SqlServer2005.ToString()] != null)
					yield return new TestCaseData(DatabaseVersion.SqlServer2005).SetCategory(DatabaseVersion.SqlServer2005.ToString());
			}
		}

		public SqlServer2005TestAttribute() : base(typeof(SqlServer2005TestAttribute), "versions")
		{ }
	}
}