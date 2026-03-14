namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20221005142800)]

	public class AddCompanyPersonRelationshipTable : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[CompanyPersonRelationship]"))
			{
				Database.AddTable(
					"[CRM].[CompanyPersonRelationship]",
					new Column("CompanyPersonRelationshipId", DbType.Guid, ColumnProperty.PrimaryKey),
					new Column("RelationshipType", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Information", DbType.String, 150, ColumnProperty.Null),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("CompanyKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("PersonKey", DbType.Guid, ColumnProperty.NotNull)
			);
				Database.ExecuteNonQuery("ALTER TABLE CRM.CompanyPersonRelationship ADD FOREIGN KEY (CompanyKey) REFERENCES CRM.Company(ContactKey)");
				Database.ExecuteNonQuery("ALTER TABLE CRM.CompanyPersonRelationship ADD FOREIGN KEY (PersonKey) REFERENCES CRM.Person(ContactKey)");
			}
		}
	}
}
