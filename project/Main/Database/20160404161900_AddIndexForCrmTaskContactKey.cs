namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161900)]
	public class AddIndexForCrmTaskContactKey : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Task]') AND name = N'IX_Task_ContactKey_ResponsibleUser_IsActive'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Task_ContactKey_ResponsibleUser_IsActive] ON [CRM].[Task]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Task_ContactKey_ResponsibleUser_IsActive] ON [CRM].[Task] ([ContactKey] ASC, [ResponsibleUser] ASC, [IsActive] ASC)");
		}
	}
}