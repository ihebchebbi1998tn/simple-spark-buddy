namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160518164700)]
	public class AddIndexesToCrmCommunication : Migration
	{
		public override void Up()
		{
			var query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_GroupKey_AddressKey')" +
													 "CREATE NONCLUSTERED INDEX [IX_GroupKey_AddressKey] ON [CRM].[Communication] ([GroupKey]) INCLUDE ([AddressKey])";

			Database.ExecuteNonQuery(query);

			query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_Communication_ContactKey')" +
													 "CREATE NONCLUSTERED INDEX [IX_Communication_ContactKey] ON [CRM].[Communication] ([ContactKey] ASC)";

			Database.ExecuteNonQuery(query);
		}
	}
}