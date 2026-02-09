namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160518165600)]
	public class AddIndexToCrmContactUser : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_ContactUser_UserName')" +
													 "CREATE NONCLUSTERED INDEX [IX_ContactUser_UserName] ON [CRM].[ContactUser] ([Username] ASC)";

			Database.ExecuteNonQuery(query);
		}
	}
}