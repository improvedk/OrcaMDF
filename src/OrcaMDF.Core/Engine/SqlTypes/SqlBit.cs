using System;
using OrcaMDF.Core.Engine.Records;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlBit : SqlTypeBase
	{
		private readonly RecordReadState readState;

		public SqlBit(RecordReadState readState, CompressionContext compression)
			: base(compression)
		{
			this.readState = readState;
		}

		public override bool IsVariableLength
		{
			get { return false; }
		}

		public override short? FixedLength
		{
			get
			{
				if (readState.AllBitsConsumed)
					return 1;

				return 0;
			}
		}

		public override object GetValue(byte[] value)
		{
			if (CompressionContext.CompressionLevel != CompressionLevel.None)
			{
				if (value.Length > 1)
					throw new ArgumentException("Invalid value length: " + value.Length);

				return value.Length == 1;
			}
			else
			{
				if (readState.AllBitsConsumed && value.Length != 1)
					throw new ArgumentException("All bits consumed, invalid value length: " + value.Length);

				if (value.Length == 1)
					readState.LoadBitByte(value[0]);

				return readState.GetNextBit();
			}
		}
	}
}