using System;
using System.Collections;
using System.Linq;
using OrcaMDF.Core.Engine.Pages;

namespace OrcaMDF.Core.Engine.Records
{
	public class PrimaryRecord : Record
	{
		public bool HasVersioningInformation { get; private set; }
		public bool IsGhostForwardedRecord { get; private set; }

		public PrimaryRecord(byte[] bytes, Page page)
			: base(page)
		{
			short offset = 0;
			
			parseStatusBitsA(new BitArray(new [] { bytes[offset++] }));

			// TODO: Strategize this stuff to avoid ifs, switches & impersonation
			if(Type == RecordType.ForwardingStub)
			{
				// Forwarding stub only has one status byte. Remaining 8 bytes are for (PageID, FileID, Slot)
				FixedLengthData = bytes.Skip(1).Take(8).ToArray();

				int pageID = BitConverter.ToInt32(bytes, 1);
				short fileID = BitConverter.ToInt16(bytes, 5);
				short slot = BitConverter.ToInt16(bytes, 7);

				var forwardPage = page.File.GetDataPage(new PagePointer(fileID, pageID));
				byte[] forwardedRecordBytes = forwardPage.Records[slot].RawBytes;

				parseStatusBitsA(new BitArray(new[] {forwardedRecordBytes[0]}));
				bytes = forwardedRecordBytes;

				// We'll impersonate the ForwardingStub record type that we originated from, this allows
				// the engine to distinguish BlobFragments and the records that actually reference them.
				Type = RecordType.ForwardingStub;
			}

			parseStatusBitsB(bytes[offset++]);

			short fixedLengthSize = BitConverter.ToInt16(bytes, offset);
			fixedLengthSize -= 4;
			offset += 2;

			switch (Type)
			{
				case RecordType.Forwarded:
					// Ignore 10 byte forwarded record header
					offset += 10;
					fixedLengthSize -= 10;

					FixedLengthData = bytes.Skip(offset).Take(fixedLengthSize).ToArray();
					offset += fixedLengthSize;
					break;

				default:
					FixedLengthData = bytes.Skip(offset).Take(fixedLengthSize).ToArray();
					offset += fixedLengthSize;
					break;
			}

			NumberOfColumns = BitConverter.ToInt16(bytes, offset);
			offset += 2;

			if (HasNullBitmap)
				offset = ParseNullBitmap(bytes, ref offset);

			if (HasVariableLengthColumns)
				ParseVariableLengthColumns(bytes, ref offset);

			// Save complete record raw bytes
			RawBytes = bytes.Take(offset).ToArray();
		}

		private void parseStatusBitsA(BitArray bits)
		{
			// Bit 0 (versioning bit) we don't care about as it's always 0 in 2k8+

			// Bits 1-3 represents record type
			Type = (RecordType)((Convert.ToByte(bits[1]) << 2) + (Convert.ToByte(bits[2]) << 1) + Convert.ToByte(bits[3]));

			// Bit 4 determines whether a null bitmap is present
			HasNullBitmap = bits[4];

			// Bit 5 determines whether there are variable length columns
			HasVariableLengthColumns = bits[5];

			// Bit 6 determines whether the row contains versioning information
			HasVersioningInformation = bits[6];

			// Bit 7 isn't used in 2k8+
		}

		private void parseStatusBitsB(byte bits)
		{
			// As the 'Ghost forwarded record' bit is the only one stored in the second byte,
			// we can simply read the whole byte value instead of extracting the first
			// bit explicitly.
			IsGhostForwardedRecord = bits == 1;
		}
	}
}