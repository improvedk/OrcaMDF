using System;

namespace OrcaMDF.Core.Framework
{
	public static class ArrayHelper
	{
		public static T[] SliceArray<T>(T[] input, int offset, int length)
		{
			T[] output = new T[length];
			Array.Copy(input, offset, output, 0, length);

			return output;
		}
	}
}