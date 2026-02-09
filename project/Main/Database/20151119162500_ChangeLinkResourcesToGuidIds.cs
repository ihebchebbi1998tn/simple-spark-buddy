namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151119162500)]
	public class ChangeLinkResourcesToGuidIds : Migration
	{
		public override void Up()
		{
      if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = 'LinkResource' AND COLUMN_NAME = 'Id' AND DATA_TYPE = 'uniqueidentifier'") == 0)
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] DROP CONSTRAINT [PK_CRM.LinkResource]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] ADD [IdOld] INT NULL");
				Database.ExecuteNonQuery("UPDATE [CRM].[LinkResource] SET [IdOld] = [Id]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] DROP COLUMN [Id]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] ADD [Id] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] ADD CONSTRAINT [PK_LinkResource] PRIMARY KEY([Id])");
			}
		}
	}
}