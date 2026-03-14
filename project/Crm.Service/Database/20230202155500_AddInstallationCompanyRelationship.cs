namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model.Relationships;

	[Migration(20230202155500)]
	public class AddInstallationCompanyRelationship : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[InstallationCompanyRelationship]"))
			{
				Database.AddTable(
					"[SMS].[InstallationCompanyRelationship]",
					new Column("InstallationCompanyRelationshipId",
						DbType.Guid,
						ColumnProperty.PrimaryKey,
						"NEWSEQUENTIALID()"),
					new Column("RelationshipType",
						DbType.String,
						20,
						ColumnProperty.NotNull),
					new Column("CreateUser",
						DbType.String,
						ColumnProperty.NotNull,
						"'Setup'"),
					new Column("ModifyUser",
						DbType.String,
						ColumnProperty.NotNull,
						"'Setup'"),
					new Column("CreateDate",
						DbType.DateTime,
						ColumnProperty.NotNull,
						"GETUTCDATE()"),
					new Column("ModifyDate",
						DbType.DateTime,
						ColumnProperty.NotNull,
						"GETUTCDATE()"),
					new Column("IsActive",
						DbType.Boolean,
						ColumnProperty.NotNull,
						true),
					new Column("Information",
						DbType.String,
						Int32.MaxValue,
						ColumnProperty.Null),
					new Column("InstallationKey",
						DbType.Guid,
						ColumnProperty.NotNull),
					new Column("CompanyKey",
						DbType.Guid,
						ColumnProperty.NotNull)
				);
				Database.ExecuteNonQuery("ALTER TABLE SMS.InstallationCompanyRelationship ADD FOREIGN KEY (InstallationKey) REFERENCES SMS.InstallationHead(ContactKey)");
				Database.ExecuteNonQuery("ALTER TABLE SMS.InstallationCompanyRelationship ADD FOREIGN KEY (CompanyKey) REFERENCES CRM.Company(ContactKey)");
			}
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<InstallationCompanyRelationship>("SMS", "InstallationCompanyRelationship");
		}
	}
}
