using System;
using OrcaMDF.Core.MetaData.Enumerations;

namespace OrcaMDF.Core.MetaData
{
	public class SysrscolTIParser
	{
		public byte Scale;
		public byte Precision;
		public short MaxLength;
		public short MaxInrowLength;
		public byte TypeID;

		public SysrscolTIParser(int ti)
		{
			TypeID = (byte)(ti & 0xFF);

			if (!Enum.IsDefined(typeof(SystemType), TypeID))
				throw new ArgumentException("Unknown TypeID '" + TypeID + "'");

			switch((SystemType)TypeID)
			{
				case SystemType.Bigint:
					MaxLength = MaxInrowLength = 8;
					Precision = 19;
					break;

				// All CLR types internally stored as varbinaries
				//case SystemType.Geography:
				//case SystemType.Geometry:
				//case SystemType.Hierarchyid:
				case SystemType.Varbinary:
				// Also covers SystemType.Sysname
				case SystemType.Nvarchar:
				case SystemType.Binary:
				case SystemType.Char:
				case SystemType.Nchar:
				case SystemType.Image:
				case SystemType.Ntext:
				case SystemType.Text:
				case SystemType.Varchar:
				case SystemType.Xml:
					MaxLength = (short)((ti & 0xFFFF00) >> 8);
					if (MaxLength == 0)
					{
						MaxLength = -1;
						MaxInrowLength = 8000;
					}
					else
						MaxInrowLength = MaxLength;
					break;

				case SystemType.Bit:
					MaxLength = MaxInrowLength = Precision = 1;
					break;

				case SystemType.Date:
					Precision = 10;
					MaxLength = MaxInrowLength = 3;
					break;

				case SystemType.Datetime:
					Scale = 3;
					Precision = 23;
					MaxLength = MaxInrowLength = 8;
					break;

				case SystemType.Datetime2:
					Scale = (byte)((ti & 0xFF00) >> 8);
					Precision = (byte)(20 + Scale);
					if (Scale < 3)
						MaxLength = MaxInrowLength = 6;
					else if (Scale < 5)
						MaxLength = MaxInrowLength = 7;
					else
						MaxLength = MaxInrowLength = 8;
					break;

				case SystemType.DatetimeOffset:
					Scale = (byte)((ti & 0xFF00) >> 8);
					Precision = (byte)(26 + (Scale > 0 ? Scale + 1 : Scale));
					if (Scale < 3)
						MaxLength = MaxInrowLength = 8;
					else if (Scale < 5)
						MaxLength = MaxInrowLength = 9;
					else
						MaxLength = MaxInrowLength = 10;
					break;

				case SystemType.Decimal:
				case SystemType.Numeric:
					Precision = (byte)((ti & 0xFF00) >> 8);
					Scale = (byte)((ti & 0xFF0000) >> 16);
					if (Precision < 10)
						MaxLength = MaxInrowLength = 5;
					else if (Precision < 20)
						MaxLength = MaxInrowLength = 9;
					else if (Precision < 29)
						MaxLength = MaxInrowLength = 13;
					else
						MaxLength = MaxInrowLength = 17;
					break;

				case SystemType.Float:
					Precision = 53;
					MaxLength = MaxInrowLength = 8;
					break;
					
				case SystemType.Int:
					Precision = 10;
					MaxLength = MaxInrowLength = 4;
					break;

				case SystemType.Money:
					Scale = 4;
					Precision = 19;
					MaxLength = MaxInrowLength = 8;
					break;

				case SystemType.Real:
					Precision = 24;
					MaxLength = MaxInrowLength = 4;
					break;

				case SystemType.Smalldatetime:
					Precision = 16;
					MaxLength = MaxInrowLength = 4;
					break;

				case SystemType.Smallint:
					Precision = 5;
					MaxLength = MaxInrowLength = 2;
					break;

				case SystemType.Smallmoney:
					Scale = 4;
					Precision = 10;
					MaxLength = MaxInrowLength = 4;
					break;

				case SystemType.Sql_Variant:
					MaxLength = MaxInrowLength = 8016;
					break;

				case SystemType.Time:
					Scale = (byte)((ti & 0xFF00) >> 8);
					Precision = (byte)(8 + (Scale > 0 ? Scale + 1 : Scale));
					if (Scale < 3)
						MaxLength = MaxInrowLength = 3;
					else if (Scale < 5)
						MaxLength = MaxInrowLength = 4;
					else
						MaxLength = MaxInrowLength = 5;
					break;

				case SystemType.Timestamp:
					MaxLength = MaxInrowLength = 8;
					break;

				case SystemType.Tinyint:
					Precision = 3;
					MaxLength = MaxInrowLength = 1;
					break;

				case SystemType.Uniqueidentifier:
					MaxLength = MaxInrowLength = 16;
					break;

				default:
					throw new ArgumentException("TypeID '" + TypeID + "' not supported.");
			}
		}
	}
}