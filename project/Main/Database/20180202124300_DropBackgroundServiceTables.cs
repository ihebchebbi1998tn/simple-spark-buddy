namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180202124300)]
	public class DropBackgroundServiceTables : Migration
	{
		public override void Up()
		{
			Database.RemoveTableIfExisting("[CRM].[BackgroundServiceSetting]");
			Database.RemoveTableIfExisting("[CRM].[BackgroundService]");
		}
	}
}