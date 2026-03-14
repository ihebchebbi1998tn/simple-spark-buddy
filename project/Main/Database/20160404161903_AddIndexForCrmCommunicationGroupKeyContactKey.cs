namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161903)]
	public class AddIndexForCrmCommunicationGroupKeyContactKey : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Communication]') AND name = N'IX_Communication_GroupKey_ContactKey'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Communication_GroupKey_ContactKey] ON [CRM].[Communication]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Communication_GroupKey_ContactKey] ON [CRM].[Communication] ([GroupKey] ASC, [ContactKey] ASC)");
		}
	}
}