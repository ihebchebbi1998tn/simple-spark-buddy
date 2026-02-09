namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190251430000)]
	public class RemoveServiceCaseInstallationNo : Migration
	{
		public override void Up()
		{
			Database.RemoveForeignKey("SMS.ServiceNotifications", "FK_ServiceNotifications_InstallationHead");
			Database.RemoveColumnIfEmpty("SMS.ServiceNotifications", "InstallationNo", null);
		}
	}
}