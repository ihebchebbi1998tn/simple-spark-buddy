namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170822153902)]
	public class UpdateSmsInstallationPosUserFlagsToNonNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
UPDATE [SMS].[InstallationPos] SET [UserFlag01] = 0 WHERE [UserFlag01] IS NULL OR [UserFlag01] <> 1
ALTER TABLE [SMS].[InstallationPos] ALTER COLUMN [UserFlag01] BIT NOT NULL
ALTER TABLE [SMS].[InstallationPos] ADD DEFAULT 0 FOR [UserFlag01]

UPDATE [SMS].[InstallationPos] SET [UserFlag02] = 0 WHERE [UserFlag02] IS NULL OR [UserFlag02] <> 1
ALTER TABLE [SMS].[InstallationPos] ALTER COLUMN [UserFlag02] BIT NOT NULL
ALTER TABLE [SMS].[InstallationPos] ADD DEFAULT 0 FOR [UserFlag02]

UPDATE [SMS].[InstallationPos] SET [UserFlag03] = 0 WHERE [UserFlag03] IS NULL OR [UserFlag03] <> 1
ALTER TABLE [SMS].[InstallationPos] ALTER COLUMN [UserFlag03] BIT NOT NULL
ALTER TABLE [SMS].[InstallationPos] ADD DEFAULT 0 FOR [UserFlag03]
");
		}
	}
}