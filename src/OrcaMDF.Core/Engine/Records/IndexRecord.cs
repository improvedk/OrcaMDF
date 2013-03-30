using System;
using System.Collections;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Engine.Records
{
	public class IndexRecord : Record
	{
		public IndexRecord(byte[] bytes, Page page)
			: base(page)
		{
			parseStatusBitsA(new BitArray(new[] { bytes[0] }));

			// Index records don't contain fixed length header - it's stored in the page header
			FixedLengthData = bytes.Skip(1).Take(Page.Header.Pminlen - 1).ToArray();
			short offset = Page.Header.Pminlen;

			NumberOfColumns = BitConverter.ToInt16(bytes, offset);
			offset += 2;

			if (HasNullBitmap)
				offset = ParseNullBitmap(bytes, ref offset);

			if (HasVariableLengthColumns)
				ParseVariableLengthColumns(bytes, ref offset);
		}

		private void parseStatusBitsA(BitArray bits)
		{
			// Bit 0 unknown - probably versioning bit as in primary records

			// Bits 1-3 represents record type
			Type = (RecordType)((Convert.ToByte(bits[1])) + (Convert.ToByte(bits[2]) << 1) + (Convert.ToByte(bits[3]) << 2));

			// Bit 4 determines whether a null bitmap is present
			HasNullBitmap = bits[4];

			// Bit 5 determines whether there are variable length columns
			HasVariableLengthColumns = bits[5];

			// Bits 6-7 not used
		}
	}
}