namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Main.Model;

	[Migration(20231010152000)]
	public class ExtendUserSkillTable : Migration {
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			if (Database.TableExists("[CRM].[UserSkill]"))
			{
				var primaryKeyConstraintName = (string)Database.ExecuteScalar(
					@"SELECT CONSTRAINT_NAME
							FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
							WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_SCHEMA = 'CRM' AND TABLE_NAME = 'UserSkill'");
				
				Database.ExecuteNonQuery($"ALTER TABLE [CRM].[UserSkill] DROP CONSTRAINT {primaryKeyConstraintName}");
				
				Database.AddColumn("[CRM].[UserSkill]", new Column("Id", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"));
				Database.AddColumn("[CRM].[UserSkill]", new Column("ValidTo", DbType.DateTime, ColumnProperty.Null));
				Database.AddColumn("[CRM].[UserSkill]", new Column("DaysToNotifyBeforeExpiration", DbType.Int32, ColumnProperty.Null));
				Database.AddColumn("[CRM].[UserSkill]", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
				Database.AddColumn("[CRM].[UserSkill]", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
				Database.AddColumn("[CRM].[UserSkill]", new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"));
				Database.AddColumn("[CRM].[UserSkill]", new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"));
				Database.AddColumn("[CRM].[UserSkill]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
				helper.AddOrUpdateEntityAuthDataColumn<UserSkill>("CRM", "UserSkill", "Id");
			}
		}
	}
}
