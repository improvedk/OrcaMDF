using System;

namespace OrcaMDF.RawCore.Types
{
	public class RawDate : RawType, IRawFixedLengthType
	{
		public short Length { get { return 3; } }

		public RawDate(string name) : base(name)
		{ }

		public override object GetValue(byte[] bytes)
		{
			// Magic needed to read a 3 byte integer into .NET's 4 byte representation.
			// Reading backwards due to assumed little endianness.
			int date = (bytes[2] << 16) + (bytes[1] << 8) + bytes[0];

			return new DateTime(1, 1, 1).AddDays(date);
		}
	}
}