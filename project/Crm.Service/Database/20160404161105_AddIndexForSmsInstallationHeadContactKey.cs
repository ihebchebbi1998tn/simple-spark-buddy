namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161105)]
	public class AddIndexForSmsInstallationHeadContactKey : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[InstallationHead]') AND name = N'IX_InstallationHead_ContactKey'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_InstallationHead_ContactKey] ON [SMS].[InstallationHead]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_InstallationHead_ContactKey] ON [SMS].[InstallationHead] ([ContactKey])");
		}
	}
}