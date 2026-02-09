namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201111123500)]
	public class AlterPKInSmsInstallationHead : Migration
	{
		public override void Up()
		{
			var query = @"
				IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [name] like 'FK_ServiceNotifications_InstallationHead')
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP CONSTRAINT [FK_ServiceNotifications_InstallationHead]
				END;

				IF COL_LENGTH('SMS.ServiceNotifications', 'InstallationNo') IS NOT NULL
				BEGIN
					ALTER TABLE [SMS].[ServiceNotifications] DROP COLUMN [InstallationNo]
				END

				IF EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('SMS.InstallationHead'))
				BEGIN
					ALTER TABLE [SMS].[InstallationHead] DROP CONSTRAINT [PK_InstallationHead]
				END;

				ALTER TABLE [SMS].[InstallationHead] ADD  CONSTRAINT [PK_InstallationHead] PRIMARY KEY CLUSTERED
				([ContactKey] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";

			Database.ExecuteNonQuery(query);
		}
	}
}