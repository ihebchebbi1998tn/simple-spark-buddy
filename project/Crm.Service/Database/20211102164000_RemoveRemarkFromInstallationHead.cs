namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211102164000)]
	public class RemoveRemarkFromInstallationHead : Migration
	{
		public override void Up()
		{
			{
				{
					Database.ExecuteNonQuery(
						@"
                 IF NOT EXISTS (SELECT * FROM [SMS].[InstallationHead] WHERE [SMS].[InstallationHead].[Remark] IS NOT NULL)
                 ALTER TABLE [SMS].[InstallationHead] DROP COLUMN [Remark]");
				}
			}
		}
	}
}
