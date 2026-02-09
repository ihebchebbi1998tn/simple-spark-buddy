namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Library.Model;

	using Main.Model;

	[Migration(20231214115200)]
	public class ExtendUserUserGroupTable : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			if (Database.TableExists("[CRM].[UserUserGroup]"))
			{
				Database.ExecuteNonQuery(@"
								IF EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND OBJECT_NAME(parent_object_id) = N'UserUserGroup')
								BEGIN
									ALTER TABLE [CRM].[UserUserGroup]
									DROP CONSTRAINT [PK_UserUserGroup];
								END");
				Database.AddColumn("[CRM].[UserUserGroup]", new Column("Id", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"));
				Database.AddColumn("[CRM].[UserUserGroup]", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
				Database.AddColumn("[CRM].[UserUserGroup]", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
				Database.AddColumn("[CRM].[UserUserGroup]", new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"));
				Database.AddColumn("[CRM].[UserUserGroup]", new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"));
				Database.AddColumn("[CRM].[UserUserGroup]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
				helper.AddOrUpdateEntityAuthDataColumn<UserUserGroup>("CRM", "UserUserGroup", "Id");
			}
		}
	}
}
