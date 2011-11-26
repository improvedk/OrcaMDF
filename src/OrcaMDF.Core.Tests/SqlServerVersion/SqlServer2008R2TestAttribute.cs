using System.Collections.Generic;
using NUnit.Framework;

namespace OrcaMDF.Core.Tests.SqlServerVersion
{
	public class SqlServer2008R2TestAttribute : TestCaseSourceAttribute
	{
		private static IEnumerable<TestCaseData> versions
		{
			get
			{
				yield return new TestCaseData(DatabaseVersion.SqlServer2008R2).SetCategory(DatabaseVersion.SqlServer2008R2.ToString());
			}
		}

		public SqlServer2008R2TestAttribute()
			: base(typeof(SqlServer2008R2TestAttribute), "versions")
		{ }
	}
}