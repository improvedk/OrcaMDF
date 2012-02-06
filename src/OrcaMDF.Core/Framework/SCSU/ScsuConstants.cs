namespace OrcaMDF.Core.Framework.SCSU
{
	/// <summary>
	/// Based on the Java reference implementation provided by Unicode, Inc:
	/// http://www.unicode.org/reports/tr6/
	/// </summary>
	internal abstract class ScsuConstants
	{
		internal const byte SQ0 = 0x01; // Quote from window pair 0
		internal const byte SQ1 = 0x02; // Quote from window pair 1
		internal const byte SQ2 = 0x03; // Quote from window pair 2
		internal const byte SQ3 = 0x04; // Quote from window pair 3
		internal const byte SQ4 = 0x05; // Quote from window pair 4
		internal const byte SQ5 = 0x06; // Quote from window pair 5
		internal const byte SQ6 = 0x07; // Quote from window pair 6
		internal const byte SQ7 = 0x08; // Quote from window pair 7
		internal const byte SDX = 0x0B; // Define a window as extended
		internal const byte Srs = 0x0C; // reserved
		internal const byte SQU = 0x0E; // Quote a single Unicode character
		internal const byte SCU = 0x0F; // Change to Unicode mode
		internal const byte SC0 = 0x10; // Select window 0
		internal const byte SC1 = 0x11; // Select window 1
		internal const byte SC2 = 0x12; // Select window 2
		internal const byte SC3 = 0x13; // Select window 3
		internal const byte SC4 = 0x14; // Select window 4
		internal const byte SC5 = 0x15; // Select window 5
		internal const byte SC6 = 0x16; // Select window 6
		internal const byte SC7 = 0x17; // Select window 7
		internal const byte SD0 = 0x18; // Define and select window 0
		internal const byte SD1 = 0x19; // Define and select window 1
		internal const byte SD2 = 0x1A; // Define and select window 2
		internal const byte SD3 = 0x1B; // Define and select window 3
		internal const byte SD4 = 0x1C; // Define and select window 4
		internal const byte SD5 = 0x1D; // Define and select window 5
		internal const byte SD6 = 0x1E; // Define and select window 6
		internal const byte SD7 = 0x1F; // Define and select window 7
		internal const byte UC0 = 0xE0; // Select window 0
		internal const byte UC1 = 0xE1; // Select window 1
		internal const byte UC2 = 0xE2; // Select window 2
		internal const byte UC3 = 0xE3; // Select window 3
		internal const byte UC4 = 0xE4; // Select window 4
		internal const byte UC5 = 0xE5; // Select window 5
		internal const byte UC6 = 0xE6; // Select window 6
		internal const byte UC7 = 0xE7; // Select window 7
		internal const byte UD0 = 0xE8; // Define and select window 0
		internal const byte UD1 = 0xE9; // Define and select window 1
		internal const byte UD2 = 0xEA; // Define and select window 2
		internal const byte UD3 = 0xEB; // Define and select window 3
		internal const byte UD4 = 0xEC; // Define and select window 4
		internal const byte UD5 = 0xED; // Define and select window 5
		internal const byte UD6 = 0xEE; // Define and select window 6
		internal const byte UD7 = 0xEF; // Define and select window 7
		internal const byte UQU = 0xF0; // Quote a single Unicode character
		internal const byte UDX = 0xF1; // Define a Window as extended
		internal const byte Urs = 0xF2; // reserved

		/*
		 * Unicode code points from 3400 to E000 are not adressible by
		 * dynamic window, since in these areas no short run alphabets are
		 * found. Therefore add gapOffset to all values from gapThreshold
		 */
		internal const int GapThreshold = 0x68;
		internal const int GapOffset = 0xAC00;

		// Values between reservedStart and fixedThreshold are reserved
		internal const int ReservedStart = 0xA8;

		// Use table of predefined fixed offsets for values from fixedThreshold
		internal const int FixedThreshold = 0xF9;

		// Constant offsets for the 8 static windows
		internal static readonly uint[] StaticOffset =
		{
			0x0000, // ASCII for quoted tags
			0x0080, // Latin - 1 Supplement (for access to punctuation)
			0x0100, // Latin Extended-A
			0x0300, // Combining Diacritical Marks
			0x2000, // General Punctuation
			0x2080, // Currency Symbols
			0x2100, // Letterlike Symbols and Number Forms
			0x3000  // CJK Symbols and punctuation
		};
		
		// Dynamic window offsets, intitialize to default values
		internal static readonly uint[] DynamicOffset =
		{
			0x0080, // Latin-1
			0x00C0, // Latin Extended A
			0x0400, // Cyrillic
			0x0600, // Arabic
			0x0900, // Devanagari
			0x3040, // Hiragana
			0x30A0, // Katakana
			0xFF00  // Fullwidth ASCII
		};

		// Table of fixed predefined Offsets, and byte values that index into
		internal static readonly uint[] FixedOffset =
		{
			0x00C0, // Latin-1 Letters + half of Latin Extended A
			0x0250, // IPA extensions
			0x0370, // Greek
			0x0530, // Armenian
			0x3040, // Hiragana
			0x30A0, // Katakana
			0xFF60  // Halfwidth Katakana
		};
	}
}