using System;
using System.Collections.Generic;
using System.Linq;
using OrcaMDF.Core.Engine;

namespace OrcaMDF.Core.MetaData.DMVs
{
	public class Object : Row
	{
		public string Name { get { return Field<string>("Name"); } private set { this["Name"] = value; } }
		public int ObjectID { get { return Field<int>("ObjectID"); } private set { this["ObjectID"] = value; } }
		public int? PrincipalID { get { return Field<int?>("PrincipalID"); } private set { this["PrincipalID"] = value; } }
		public int SchemaID { get { return Field<int>("SchemaID"); } private set { this["SchemaID"] = value; } }
		public int ParentObjectID { get { return Field<int>("ParentObjectID"); } private set { this["ParentObjectID"] = value; } }
		public string Type { get { return Field<string>("Type"); } private set { this["Type"] = value; } }
		public string TypeDesc { get { return Field<string>("TypeDesc"); } private set { this["TypeDesc"] = value; } }
		public DateTime CreateDate { get { return Field<DateTime>("CreateDate"); } private set { this["CreateDate"] = value; } }
		public DateTime ModifyDate { get { return Field<DateTime>("ModifyDate"); } private set { this["ModifyDate"] = value; } }
		public bool IsMSShipped { get { return Field<bool>("IsMSShipped"); } private set { this["IsMSShipped"] = value; } }
		public bool IsPublished { get { return Field<bool>("IsPublished"); } private set { this["IsPublished"] = value; } }
		public bool IsSchemaPublished { get { return Field<bool>("IsSchemaPublished"); } private set { this["IsSchemaPublished"] = value; } }

		public Object()
		{
			Columns.Add(new DataColumn("Name", "sysname"));
			Columns.Add(new DataColumn("ObjectID", "int"));
			Columns.Add(new DataColumn("PrincipalID", "int", true));
			Columns.Add(new DataColumn("SchemaID", "int"));
			Columns.Add(new DataColumn("ParentObjectID", "int"));
			Columns.Add(new DataColumn("Type", "char(2)"));
			Columns.Add(new DataColumn("TypeDesc", "nvarchar", true));
			Columns.Add(new DataColumn("CreateDate", "datetime"));
			Columns.Add(new DataColumn("ModifyDate", "datetime"));
			Columns.Add(new DataColumn("IsMSShipped", "bit"));
			Columns.Add(new DataColumn("IsPublished", "bit"));
			Columns.Add(new DataColumn("IsSchemaPublished", "bit"));
		}

		public override Row NewRow()
		{
			return new Object();
		}

		internal static IEnumerable<Object> GetDmvData(Database db)
		{
			// SQL Server gets data from sys.objects$. We need to get directly from sys.sysschobjs to avoid a stack overflow
			return db.Dmvs.ObjectsDollar
				.Select(o => new Object
				    {
				        Name = o.Name,
				        ObjectID = o.ObjectID,
						PrincipalID = o.PrincipalID,
				        SchemaID = o.SchemaID,
				        ParentObjectID = o.ParentObjectID,
				        Type = o.Type.Trim(),
						TypeDesc = o.TypeDesc,
				        CreateDate = o.CreateDate,
				        ModifyDate = o.ModifyDate,
						IsMSShipped = o.IsMSShipped,
						IsPublished = o.IsPublished,
						IsSchemaPublished = o.IsSchemaPublished
				    });
		}
	}
}