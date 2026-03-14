namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161902)]
	public class AddIndexForCrmContactParentKey : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'IX_ParentKey_ContactType'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_ParentKey_ContactType] ON [CRM].[Contact]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ParentKey_ContactType] ON [CRM].[Contact] ([ParentKey] ASC, [ContactType] ASC)");
		}
	}
}