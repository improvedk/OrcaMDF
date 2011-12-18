using System;
using OrcaMDF.Core.Engine.Records;

namespace OrcaMDF.Core.Engine.SqlTypes
{
	public class SqlBit : ISqlType
	{
		private readonly RecordReadState readState;

		public SqlBit(RecordReadState readState)
		{
			this.readState = readState;
		}

		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get
			{
				if (readState.AllBitsConsumed)
					return 1;

				return 0;
			}
		}

		public byte[] NormalizeCompressedValue(byte[] value)
		{
			throw new NotImplementedException();
		}

		public object GetValue(byte[] value)
		{
			if(readState.AllBitsConsumed && value.Length != 1)
				throw new ArgumentException("All bits consumed, invalid value length: " + value.Length);

			if (value.Length == 1)
				readState.LoadBitByte(value[0]);

			return readState.GetNextBit();
		}
	}
}