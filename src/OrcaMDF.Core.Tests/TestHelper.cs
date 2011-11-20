using System.Reflection;

namespace OrcaMDF.Core.Tests
{
	internal static class TestHelper
	{
		internal static void GetAllPublicProperties(object obj)
		{
			var props = obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

			foreach(var prop in props)
				prop.GetValue(obj, null);
		}
	}
}