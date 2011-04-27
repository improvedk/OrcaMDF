namespace OrcaMDF.Core.SqlTypes
{
	// TODO: This is fugly. Rewrite this logic so it isn't dependent on factory but uses a per-record temporary bit read state.
	public class SqlBit : ISqlType
	{
		private SqlTypeFactory factory;

		public SqlBit(SqlTypeFactory factory)
		{
			this.factory = factory;
		}

		public bool IsVariableLength
		{
			get { return false; }
		}

		public short? FixedLength
		{
			get
			{
				if (factory.BitReadState == null || factory.BitReadState.AllBitsConsumed)
					return 1;

				return 0;
			}
		}

		public object GetValue(byte[] value)
		{
			if (value.Length == 1)
				factory.BitReadState = new SqlBitReadState(value[0]);

			return factory.BitReadState.GetNextBit();
		}
	}
}