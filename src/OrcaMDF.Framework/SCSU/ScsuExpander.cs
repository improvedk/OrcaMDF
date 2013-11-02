using OrcaMDF.Framework.SCSU.Exceptions;
using System.Text;

namespace OrcaMDF.Framework.SCSU
{
	/// <summary>
	/// Based on the Java reference implementation provided by Unicode, Inc:
	/// http://www.unicode.org/reports/tr6/
	/// </summary>
	public class ScsuExpander
	{
		// The currently active dynamic window
		private uint activeWindow;

		/// <summary>
		/// (re-)define (and select) a dynamic window
		/// A sliding window position cannot start at any Unicode value,
		///	so rather than providing an absolute offset, this function takes
		///	an index value which selects among the possible starting values.
		///
		///	Most scripts in Unicode start on or near a half-block boundary
		///	so the default behaviour is to multiply the index by 0x80. Han,
		///	Hangul, Surrogates and other scripts between 0x3400 and 0xDFFF
		///	show very poor locality--therefore no sliding window can be set
		///	there. A jumpOffset is added to the index value to skip that region,
		///	and only 167 index values total are required to select all eligible
		///	half-blocks.
		///
		///	Finally, a few scripts straddle half block boundaries. For them, a
		///	table of fixed offsets is used, and the index values from 0xF9 to
		///	0xFF are used to select these special offsets.
		///
		///	After (re-)defining a windows location it is selected so it is ready
		///	for use.
		///
		///	Recall that all Windows are of the same length (128 code positions).
		/// </summary>
		/// <param name="window">Index of the window to be (re-)defined</param>
		/// <param name="offset">Index for the new offset value</param>
		private void defineWindow(uint window, byte offset)
		{
			uint iOffset = offset;

			// 0 is a reserved value
			if (iOffset == 0)
				throw new IllegalInputException();

			if (iOffset < ScsuConstants.GapThreshold)
				ScsuConstants.DynamicOffset[window] = iOffset << 7;
			else if (iOffset < ScsuConstants.ReservedStart)
				ScsuConstants.DynamicOffset[window] = (iOffset << 7) + ScsuConstants.GapOffset;
			else if (iOffset < ScsuConstants.FixedThreshold)
				throw new IllegalInputException("iOffset == " + iOffset);
			else
				ScsuConstants.DynamicOffset[window] = ScsuConstants.FixedOffset[iOffset - ScsuConstants.FixedThreshold];

			// Make the redefined window the active one
			activeWindow = window;
		}

		/// <summary>
		/// (re-)define (and select) a window as an extended dynamic window
		/// The surrogate area in Unicode allows access to 2**20 codes beyond the
		/// first 64K codes by combining one of 1024 characters from the High
		/// Surrogate Area with one of 1024 characters from the Low Surrogate
		/// Area (see Unicode 2.0 for the details).
		/// 
		/// The tags SDX and UDX set the window such that each subsequent byte in
		/// the range 80 to FF represents a surrogate pair. The following diagram
		/// shows how the bits in the two bytes following the SDX or UDX, and a
		/// subsequent data byte, map onto the bits in the resulting surrogate pair.
		/// 
		/// hbyte         lbyte          data
		/// nnnwwwww      zzzzzyyy      1xxxxxxx
		/// 
		/// high-surrogate     low-surrogate
		/// 110110wwwwwzzzzz   110111yyyxxxxxxx
		/// </summary>
		/// <param name="offset">
		///	offset - Since the three top bits of offset are not needed to
		///	set the location of the extended Window, they are used instead
		///	to select the window, thereby reducing the number of needed command codes.
		///	The bottom 13 bits of offset are used to calculate the offset relative to
		///	a 7 bit input data byte to yield the 20 bits expressed by each surrogate pair.
		/// </param>
		private void defineExtendedWindow(uint offset)
		{
			// The top 3 bits of iOffsetHi are the window index
			uint iWindow = offset >> 13;

			// Calculate the new offset
			ScsuConstants.DynamicOffset[iWindow] = ((offset & 0x1FFF) << 7) + (1 << 16);

			// Make the redefined window the active one
			activeWindow = iWindow;
		}

		// String buffer length used by the following functions
		private int outputLength;

		/// <summary>
		/// Expand input that is in Unicode mode
		/// </summary>
		/// <param name="input">Input byte array to be expanded</param>
		/// <param name="index">Starting index</param>
		/// <param name="sb">String buffer to which to append expanded input</param>
		/// <returns>The index for the lastc byte processed</returns>
		private int expandUnicode(byte[] input, int index, StringBuilder sb)
		{
			for( ; index < input.Length-1; index += 2 ) // Step by 2:
			{
				byte b = input[index];

				if (b >= ScsuConstants.UC0 && b <= ScsuConstants.UC7)
				{
					activeWindow = (uint)(b - ScsuConstants.UC0);
					return index;
				}

				if (b >= ScsuConstants.UD0 && b <= ScsuConstants.UD7)
				{
					defineWindow((uint)(b - ScsuConstants.UD0), input[index + 1]);
					return index + 1;
				}

				if (b == ScsuConstants.UDX)
				{
					if( index >= input.Length - 2)
						break; // Buffer error

					defineExtendedWindow(charFromTwoBytes(input[index + 1], input[index + 2]));
					return index + 2;
				}

				if (b == ScsuConstants.UQU)
				{
					if (index >= input.Length - 2)
						break; // Error

					// Skip command byte and output Unicode character
					index++;
				}

				// Output a Unicode character
				sb.Append(charFromTwoBytes(input[index], input[index + 1]));
				outputLength++;
			}

			if (index == input.Length)
				return index;

			// Error condition
			throw new EndOfInputException();
		}

		/// <summary>
		/// Assemble a char from two bytes
		/// </summary>
		private char charFromTwoBytes(byte hi, byte lo)
		{
			char ch = (char)lo;
			return (char)(ch + (char)(hi << 8));
		}

		/// <summary>
		/// Expand portion of the input that is in single byte mode
		/// </summary>
		private string expandSingleByte(byte[] input)
		{
			/* Allocate the output buffer. Because of control codes, generally
			each byte of input results in fewer than one character of
			output. Using input.Length as an intial allocation length should avoid
			the need to reallocate in mid-stream. The exception to this rule are
			surrogates. */
			var sb = new StringBuilder(input.Length);
			outputLength = 0;

			// Loop until all input is exhausted or an error occurred
			int index;
			for (index = 0; index < input.Length; index++)
			{
				// Default behaviour is that ASCII characters are passed through
				// (staticOffset[0] == 0) and characters with the high bit on are
				// offset by the current dynamic (or sliding) window (this.window)
				int iStaticWindow = 0;
				uint iDynamicWindow = activeWindow;

				switch (input[index])
				{
					// Quote from a static Window
					case ScsuConstants.SQ0:
					case ScsuConstants.SQ1:
					case ScsuConstants.SQ2:
					case ScsuConstants.SQ3:
					case ScsuConstants.SQ4:
					case ScsuConstants.SQ5:
					case ScsuConstants.SQ6:
					case ScsuConstants.SQ7:
						// Skip the command byte and check for length
						if (index >= input.Length - 1)
							continue;

						// Select window pair to quote from
						iDynamicWindow = (uint)(iStaticWindow = input[index] - ScsuConstants.SQ0);
						index ++;

						// Fall through
						goto default;

					default:
						uint ch;

						// Output as character
						if (input[index] < 128)
						{
							// Use static window
							ch = input[index] + ScsuConstants.StaticOffset[iStaticWindow];
							sb.Append((char)ch);
							outputLength++;
						}
						else
						{
							// Use dynamic window
							ch = input[index];
							ch -= 0x80;                // Reduce to range 00..7F
							ch += ScsuConstants.DynamicOffset[iDynamicWindow];
							
							if (ch < (1 << 16))
							{
								// In Unicode range, output directly
								sb.Append((char)ch);
								outputLength++;
							}
							else
							{
								// Compute and append the two surrogates:
								// Translate from 10000..10FFFF to 0..FFFFF
								ch -= 0x10000;

								// High surrogate = top 10 bits added to D800
								sb.Append((char)(0xD800 + (ch >> 10)));
								outputLength++;

								// Low surrogate = bottom 10 bits added to DC00
								sb.Append((char)(0xDC00 + (ch & ~0xFC00)));
								outputLength++;
							}
						}
						break;

					// Define a dynamic window as extended
					case ScsuConstants.SDX:
						index += 2;
						if (index >= input.Length)
							continue;

						defineExtendedWindow(charFromTwoBytes(input[index - 1], input[index]));
						break;

					// Position a dynamic Window
					case ScsuConstants.SD0:
					case ScsuConstants.SD1:
					case ScsuConstants.SD2:
					case ScsuConstants.SD3:
					case ScsuConstants.SD4:
					case ScsuConstants.SD5:
					case ScsuConstants.SD6:
					case ScsuConstants.SD7:
						index ++;
						if (index >= input.Length)
							continue;

						defineWindow((uint)(input[index - 1] - ScsuConstants.SD0), input[index]);
						break;

					// Select a new dynamic Window
					case ScsuConstants.SC0:
					case ScsuConstants.SC1:
					case ScsuConstants.SC2:
					case ScsuConstants.SC3:
					case ScsuConstants.SC4:
					case ScsuConstants.SC5:
					case ScsuConstants.SC6:
					case ScsuConstants.SC7:
						activeWindow = (uint)(input[index] - ScsuConstants.SC0);
						break;

					case ScsuConstants.SCU:
						// Switch to Unicode mode and continue parsing
						index = expandUnicode(input, index + 1, sb);
						break;

					case ScsuConstants.SQU:
						// Directly extract one Unicode character
						index += 2;
						if (index >= input.Length)
							continue;

						char cha = charFromTwoBytes(input[index - 1], input[index]);

						sb.Append(cha);
						outputLength++;
						break;

					case ScsuConstants.Srs:
						throw new IllegalInputException();
				}
			}

			if (index >= input.Length)
			{
				//SUCCESS: all input used up
				sb.Length = outputLength;
				return sb.ToString();
			}

			//ERROR: premature end of input
			throw new EndOfInputException();
		}

		/// <summary>
		/// Expand a byte array containing compressed Unicode
		/// </summary>
		public string Expand (byte[] input)
		{
			return expandSingleByte(input);
		}
	}
}