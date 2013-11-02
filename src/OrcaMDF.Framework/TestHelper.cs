using System;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace OrcaMDF.Framework
{
	public static class TestHelper
	{
		public static void GetAllPublicProperties(object obj)
		{
			var props = obj.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

			foreach(var prop in props)
				prop.GetValue(obj, null);
		}

		public static byte[] GetBytesFromByteString(string input)
		{
			input = input.Replace(" ", "");

			if(input.Length % 2 != 0)
				throw new FormatException("input");

			return SoapHexBinary.Parse(input).Value;
		}
	}
}