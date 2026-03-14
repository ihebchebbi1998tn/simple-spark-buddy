namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170102181700)]
	public class ChangeTagsToGuidIds : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = 'ContactTags' AND COLUMN_NAME = 'Id' AND DATA_TYPE = 'uniqueidentifier'") == 0)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_ContactTags_TagName] ON [CRM].[ContactTags]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[ContactTags] DROP CONSTRAINT [PK_ContactTags]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[ContactTags] ADD [IdOld] INT NULL");
				Database.ExecuteNonQuery("UPDATE [CRM].[ContactTags] SET [IdOld] = [Id]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[ContactTags] DROP COLUMN [Id]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[ContactTags] ADD [Id] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[ContactTags] ADD CONSTRAINT [PK_ContactTags] PRIMARY KEY([Id])");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ContactTags_TagName] ON [CRM].[ContactTags] ([TagName] ASC)");
			}
		}
	}
}