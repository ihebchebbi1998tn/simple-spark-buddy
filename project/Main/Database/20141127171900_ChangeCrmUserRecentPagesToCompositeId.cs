namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20141127171900)]
	public class ChangeCrmUserRecentPagesToCompositeId : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[CRM].[UserRecentPages]", "[Id]"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[UserRecentPages] DROP CONSTRAINT [PK__UserRecentPages]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[UserRecentPages] DROP CONSTRAINT [UK_Url_Username]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[UserRecentPages] DROP COLUMN [Id]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[UserRecentPages] ADD CONSTRAINT [PK_UserRecentPages] PRIMARY KEY ([Username], [Url])");
			}
		}
	}
}