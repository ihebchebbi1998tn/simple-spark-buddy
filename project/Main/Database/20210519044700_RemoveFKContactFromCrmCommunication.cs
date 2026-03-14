namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210519044700)]
	public class RemoveFKContactFromCrmCommunication : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE Name IN ('FK_Communication_ContactKey', 'FK_Communication_Contact')") == 2)
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Communication] DROP CONSTRAINT [FK_Communication_Contact]");
			}
		}
	}
}
