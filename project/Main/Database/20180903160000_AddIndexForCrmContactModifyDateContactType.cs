namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180903160000)]
	public class AddIndexForCrmContactModifyDateContactType : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'IX_Contact_ContactType_ModifyDate'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Contact_ContactType_ModifyDate] ON [CRM].[Contact]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Contact_ContactType_ModifyDate] ON[CRM].[Contact]([ContactType], [ModifyDate])");
		}
	}
}
