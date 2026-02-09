namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160809161600)]
	public class AddIndexToCrmContact : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Contact]') AND name = N'IX_Contact_ContactType_ResponsibleUser'") > 0)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Contact_ContactType_ResponsibleUser] ON [CRM].[Contact]");
			}
			Database.ExecuteNonQuery(@"
				CREATE NONCLUSTERED INDEX [IX_Contact_ContactType_ResponsibleUser]
					ON [CRM].[Contact] ([ContactType],[ResponsibleUser])
					INCLUDE ([ContactId],[ParentKey])
			");
		}
	}
}
