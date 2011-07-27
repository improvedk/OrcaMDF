using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;
using OrcaMDF.Core.Engine.Records.VariableLengthDataProxies;

namespace OrcaMDF.Core.Engine.Records
{
	public abstract class Record
	{
		public RecordType Type { get; protected set; }
		public bool HasNullBitmap { get; protected set; }
		public bool HasVariableLengthColumns { get; protected set; }
		public byte[] FixedLengthData { get; protected set; }
		public short NumberOfColumns { get; protected set; }
		public BitArray NullBitmap { get; protected set; }
		public short NumberOfVariableLengthColumns { get; protected set; }
		public byte[] RawBytes { get; protected set; }
		public SparseVectorParser SparseVector { get; private set; }
		public IDictionary<int, IVariableLengthDataProxy> VariableLengthColumnData { get; set; }

		protected Page Page;
		protected IDictionary<int, byte[]> RawVariableLengthColumnData { get; set; }

		protected Record(Page page)
		{
			Page = page;

			// Initialize variable length data dictionaries
			RawVariableLengthColumnData = new Dictionary<int, byte[]>();
			VariableLengthColumnData = new Dictionary<int, IVariableLengthDataProxy>();
		}

		protected void ParseVariableLengthColumns(byte[] bytes, ref short offset)
		{
			// If there is no fixed length data and no null bitmap, only the number of variable length columns is stored.
			if (FixedLengthData.Length == 0 && !HasNullBitmap)
				NumberOfVariableLengthColumns = NumberOfColumns;
			else
			{
				NumberOfVariableLengthColumns = BitConverter.ToInt16(bytes, offset);
				offset += 2;
			}

			short[] variableLengthColumnLengths = new short[NumberOfVariableLengthColumns];
			for (int i = 0; i < NumberOfVariableLengthColumns; i++)
			{
				variableLengthColumnLengths[i] = BitConverter.ToInt16(bytes, offset);
				offset += 2;
			}

			// Loop variable length columns
			for(int i=0; i<NumberOfVariableLengthColumns; i++)
			{
				// The high order bit is used to indicate a complex column (or a row-overflow pointer).
				bool complexColumn = false;
				if ((variableLengthColumnLengths[i] & 32768) == 32768)
				{
					// Flip the sign bit && remember that this is a complex column
					variableLengthColumnLengths[i] = (short)(variableLengthColumnLengths[i] & Int16.MaxValue);
					complexColumn = true;
				}

				RawVariableLengthColumnData[i] = bytes.Skip(offset).Take(variableLengthColumnLengths[i] - offset).ToArray();
				offset = variableLengthColumnLengths[i];

				// Complex columns store special values and may need to be read elsewhere. In this case I'm using somewhat of a hack to detect
				// row-overflow pointers the same way as normal complex columns. See http://improve.dk/archive/2011/07/15/identifying-complex-columns-in-records.aspx
				// for a better description of the issue. Currently there are three cases:
				// - Back pointers (two-byte value of 1024)
				// - Sparse vectors (two-byte value of 5)
				// - Row-overflow pointer (one-byte value of 2)
				// As all of these differ by just the first byte (a value of 4, 5 and 2 respectively), I'm using this to detect the
				// complex column type. The only way around this, as I see it, would be to know the scema and thus know what to look for/expect.
				// As I don't want to refactor that yet, hack away!
				// Finally complex columns also store 16 byte LOB pointers. Since these are the only 16-byte length complex columns we'll use that fact
				// to detect them and retrieve the referenced data.
				if (complexColumn)
				{
					// If length == 16 then we're dealing with a LOB pointer, otherwise it's a regular complex column
					if (RawVariableLengthColumnData[i].Length == 16)
						VariableLengthColumnData[i] = new LobDataProxy(Page, RawVariableLengthColumnData[i]);
					else
					{
						byte complexColumnID = RawVariableLengthColumnData[i][0];

						switch (complexColumnID)
						{
							// Row-overflow pointer, get referenced data
							case 0x02:
								VariableLengthColumnData[i] = new OverflowDataProxy(Page, RawVariableLengthColumnData[i]);
								break;

							// Forwarded record back pointer (http://improve.dk/archive/2011/06/09/anatomy-of-a-forwarded-record-ndash-the-back-pointer.aspx)
							// Ensure we expect a back pointer at this location. For forwarding stubs, the data stems from the referenced forwarded record. For the forwarded record,
							// the last varlength column is a backpointer. No public option for accessing raw bytes.
							case 0x04:
								if ((Type != RecordType.ForwardingStub || Type != RecordType.BlobFragment) || i != NumberOfVariableLengthColumns - 1)
									throw new ArgumentException("Unexpected back pointer found at column index " + i);
								break;

							// Sparse vectors will be processed at a later stage - no public option for accessing raw bytes
							case 0x05:
								SparseVector = new SparseVectorParser(RawVariableLengthColumnData[i]);
								break;

							default:
								throw new ArgumentException("Invalid complex column ID encountered: 0x" + BitConverter.ToInt16(RawVariableLengthColumnData[i], 0).ToString("X"));
						}
					}
				}
				else
					VariableLengthColumnData[i] = new RawByteProxy(RawVariableLengthColumnData[i]);
			}
		}

		protected short ParseNullBitmap(byte[] bytes, ref short offset)
		{
			NullBitmap = new BitArray(bytes.Skip(offset).Take((NumberOfColumns + 7)/8).ToArray());
			offset += (short)((NumberOfColumns + 7) / 8);
			return offset;
		}
	}
}