namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131029143053)]
	public class AddContactKeyIndexToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name='IX_InstallationHead_ContactKey' AND object_id = OBJECT_ID('SMS.InstallationHead')) " +
															 "BEGIN " +
															 "CREATE NONCLUSTERED INDEX [IX_InstallationHead_ContactKey] ON [SMS].[InstallationHead] " +
															 "([ContactKey] ASC)" +
			                         "WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
															 "END");
		}
		public override void Down()
		{
		}
	}
}