namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160518165100)]
	public class DropAndRecreateIndexInCrmUserRecentPages : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_UserRecentPages_Username')" +
													 "CREATE NONCLUSTERED INDEX [IX_UserRecentPages_Username] ON [CRM].[UserRecentPages] ([Username] ASC)" +
			                     "INCLUDE ([ModifyDate], [Title], [Url])";

			Database.ExecuteNonQuery(query);
		}
	}
}