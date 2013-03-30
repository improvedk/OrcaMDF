using System;
using System.Collections.Generic;
using OrcaMDF.Core.Engine.Records;
using OrcaMDF.Core.Framework;

namespace OrcaMDF.Core.Engine.Pages
{
	internal class PrimaryRecordPage : RecordPage
	{
		internal PrimaryRecord[] Records { get; set; }

		protected CompressionContext CompressionContext;

		internal PrimaryRecordPage(byte[] bytes, CompressionContext compression, Database database)
			: base(bytes, database)
		{
			CompressionContext = compression;

			parseRecords();
		}

		private void parseRecords()
		{
			//Records = new PrimaryRecord[Header.SlotCnt];
			
			//int cnt = 0;
			//foreach (short recordOffset in SlotArray)
			//	Records[cnt++] = new PrimaryRecord(ArrayHelper.SliceArray(RawBytes, recordOffset, RawBytes.Length - recordOffset), this);

			var records = new List<PrimaryRecord>();

			// Get the known records
			foreach (short recordOffset in SlotArray)
				records.Add(new PrimaryRecord(ArrayHelper.SliceArray(RawBytes, recordOffset, RawBytes.Length - recordOffset), this));

			// Remove header and record offset array from raw bytes
			// For this, we'll skip the header and the record offset array
			var bytesWithPotentialGhostData = ArrayHelper.SliceArray(RawBytes, 96, RawBytes.Length - 96 - Header.SlotCnt * 2);

			// Brute force all the bytes, looking for an interesting set of status bytes
			// Criterias are that A.Version = 0, A.RecordType = 6 (GhostData), A.HasVersioningInformation = 0 and A.NotUsed = 0 while B should be all 0
			for (int i = 0; i < bytesWithPotentialGhostData.Length - 1; i++)
			{
				// A check
				int a = bytesWithPotentialGhostData[i];

				// 0b11001111 - This clears out the bits we don't care about (null bitmap, varlength)
				a = a & 0xCF;
				
				// 0b00001100 - At this point only the record type bits should be set, indicating a GhostData record.
				// If not, we'll skip this byte and head on
				if (a != 0x0C)
					continue;

				// If the B byte is non-zero, these are not the bytes we're looking for
				if (bytesWithPotentialGhostData[i+1] != 0)
					continue;

				// At this point this could actually be a dropped record. Let's try crossing our fingers and parsing it.
				// If we crash, we'll skip it and continue on. If it's parsed as a valid PrimaryRecord, hey, let's go with it.
				try
				{
					var potentialRecord = new PrimaryRecord(ArrayHelper.SliceArray(bytesWithPotentialGhostData, i, bytesWithPotentialGhostData.Length - i), this);
					
					// If it's not a GhostData record, let's abort
					if (potentialRecord.Type != RecordType.GhostData)
						continue;

					// So the record parses, but is it even slightly realistic? Let's do some simple sanity checking
					var numberOfFixedLengthBytes = (potentialRecord.FixedLengthData ?? new byte[0]).Length;
					var numberOfFixedLengthColumns = potentialRecord.NumberOfColumns - potentialRecord.NumberOfVariableLengthColumns;

					// How can there be fixed length columns with no data?
					if (numberOfFixedLengthBytes == 0 && numberOfFixedLengthColumns > 0)
						continue;

					// How can there be fixed length data with no columns?
					if (numberOfFixedLengthBytes > 0 && numberOfFixedLengthColumns == 0)
						continue;

					// If the average number of bytes per fixed length columns seems suspicious, it probably is
					if (numberOfFixedLengthColumns > 0 && (double)numberOfFixedLengthBytes / numberOfFixedLengthColumns < 2)
						continue;

					records.Add(potentialRecord);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Exception while trying to revive the dead: " + ex.Message);
				}
			}

			// Store as array
			Records = records.ToArray();
		}
	}
}