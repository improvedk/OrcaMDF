using System;

namespace OrcaMDF.RawCore
{
	public static class ArraySegmentExtensions
	{
		public static T[] ToArray<T>(this ArraySegment<T> segment)
		{
			var array = new T[segment.Count];
			Array.Copy(segment.Array, segment.Offset, array, 0, segment.Count);

			return array;
		}
	}
}