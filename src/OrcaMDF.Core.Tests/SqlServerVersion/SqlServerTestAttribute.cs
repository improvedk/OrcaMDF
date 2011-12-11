using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;

namespace OrcaMDF.Core.Tests.SqlServerVersion
{
	public class SqlServerTestAttribute : TestCaseSourceAttribute
	{
		private static IEnumerable<TestCaseData> versions
		{
			get
			{
				foreach (var value in Enum.GetValues(typeof(DatabaseVersion)))
				{
					if(ConfigurationManager.ConnectionStrings[value.ToString()] != null)
						yield return new TestCaseData(value).SetCategory(value.ToString());
				}
			}
		}

		public SqlServerTestAttribute() : base(typeof(SqlServerTestAttribute), "versions")
		{ }
	}
}