using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcaMDF.Core.Engine
{
	/// <summary>
	/// Parses sparse vectors as stored in records for tables containing sparse columns.
	/// See http://improve.dk/archive/2011/07/15/sparse-column-storage-ndash-the-sparse-vector.aspx
	/// </summary>
	public class SparseVectorParser
	{
		public short ColumnCount { get; private set; }
		public IDictionary<short, byte[]> ColumnValues { get; private set; }

		public SparseVectorParser(byte[] bytes)
		{
			// First two bytes must have the value 5, indicating this is a sparse vector
			short complexColumnID = BitConverter.ToInt16(bytes, 0);
			if (complexColumnID != 5)
				throw new ArgumentException("Input bytes does not contain a sparse vector.");

			// Number of columns contained in this sparse vector
			ColumnCount = BitConverter.ToInt16(bytes, 2);

			// For each column, read the data into the columnValues dictionary
			ColumnValues = new Dictionary<short, byte[]>();
			short columnIDSetOffset = 4;
			short columnOffsetTableOffset = (short)(columnIDSetOffset + 2 * ColumnCount);
			short columnDataOffset = (short)(columnOffsetTableOffset + 2 * ColumnCount);
			for(int i=0; i<ColumnCount; i++)
			{
				// Read ID, data offset and data from vector
				short columnID = BitConverter.ToInt16(bytes, columnIDSetOffset);
				short columnOffset = BitConverter.ToInt16(bytes, columnOffsetTableOffset);
				byte[] data = bytes.Take(columnOffset).Skip(columnDataOffset).ToArray();

				// Add ID + data to dictionary
				ColumnValues.Add(columnID, data);

				// Increment both ID and offset offsets by two bytes
				columnIDSetOffset += 2;
				columnOffsetTableOffset += 2;
				columnDataOffset = columnOffset;
			}
		}
	}
}