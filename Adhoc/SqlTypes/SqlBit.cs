namespace Orca.MdfReader.Adhoc.SqlTypes
{
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