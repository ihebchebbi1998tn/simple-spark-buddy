namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240524092800)]
	public class DropTableNotificationStandardAction : Migration
	{
		public override void Up()
		{
			Database.RemoveTableIfExisting("[SMS].[NotificationStandardAction]");
		}
	}
}
