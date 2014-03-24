using System;
using System.Collections;
using System.Collections.Generic;

namespace OrcaMDF.Framework
{
	/// <summary>
	/// A smarter ArraySegment&lt;T&gt;
	/// </summary>
	public class ArrayDelimiter<T> : IEnumerable<T>
	{
		public int Count { get; private set; }
		public int Offset { get; private set; }
		public T[] SourceArray { get; private set; }

		public ArrayDelimiter(T[] array)
		{
			Count = array.Length;
			Offset = 0;
			SourceArray = array;
		}

		public ArrayDelimiter(T[] array, int offset, int count)
		{
			SourceArray = array;
			Offset = offset;
			Count = count;

			if (offset + count > array.Length)
				throw new IndexOutOfRangeException("Offset '" + offset + "' + count '" + count + "' exceeds the length of the source array.");
		}
		
		public T this[int index]
		{
			get
			{
				if (index + 1 > Count)
					throw new IndexOutOfRangeException("Index '" + index + "' exceeds the length of the array.");

				if (index < 0)
					throw new IndexOutOfRangeException("Negative index '" + index + "' is not supported.");

				return SourceArray[Offset + index];
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}