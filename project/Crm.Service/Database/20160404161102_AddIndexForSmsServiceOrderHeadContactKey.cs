namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161102)]
	public class AddIndexForSmsServiceOrderHeadContactKey : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_ContactKey'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_ContactKey] ON [SMS].[ServiceOrderHead]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ContactKey] ON [SMS].[ServiceOrderHead] ([ContactKey])");
		}
	}
}