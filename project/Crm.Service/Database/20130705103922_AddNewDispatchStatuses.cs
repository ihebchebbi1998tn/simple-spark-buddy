namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130705103922)]
	public class AddNewDispatchStatuses : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[ServiceOrderDispatchStatus] WHERE [Value] = 'Read'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderDispatchStatus] SET SortOrder = SortOrder + 1 WHERE SortOrder > 2");
				Database.ExecuteNonQuery("INSERT INTO [SMS].[ServiceOrderDispatchStatus] " +
				                         "([Language], [Name], [Value], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
				                         "VALUES ('de', 'Gelesen', 'Read', 0, 3, GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
				Database.ExecuteNonQuery("INSERT INTO [SMS].[ServiceOrderDispatchStatus] " +
				                         "([Language], [Name], [Value], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
				                         "VALUES ('en', 'Read', 'Read', 0, 3, GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
			}
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[ServiceOrderDispatchStatus] WHERE [Value] = 'Rejected'") == 0)
			{
				Database.ExecuteNonQuery("INSERT INTO [SMS].[ServiceOrderDispatchStatus] " +
				                         "([Language], [Name], [Value], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
				                         "VALUES ('de', 'Zurückgewiesen', 'Rejected', 0, 8, GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
				Database.ExecuteNonQuery("INSERT INTO [SMS].[ServiceOrderDispatchStatus] " +
				                         "([Language], [Name], [Value], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
				                         "VALUES ('en', 'Rejected', 'Rejected', 0, 8, GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
			}
		}
		public override void Down()
		{
		}
	}
}