namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230825110500)]
	public class IncreaseRecentPagesTitleLength : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[CRM].[UserRecentPages]", "Title"))
			{
				Database.ExecuteNonQuery("IF EXISTS (SELECT * FROM sysindexes WHERE id=object_id('[CRM].[UserRecentPages]') AND name= 'IX_UserRecentPages_Username') DROP INDEX [IX_UserRecentPages_Username] ON [CRM].[UserRecentPages]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[UserRecentPages] ALTER COLUMN [Title] NVARCHAR(500)");
				Database.ExecuteNonQuery("CREATE INDEX [IX_UserRecentPages_Username] ON [CRM].[UserRecentPages] ([Username]) INCLUDE ([Title], [Url], [ModifyDate])");
			}
		}
	}
}
