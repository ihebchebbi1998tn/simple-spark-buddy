namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180820092600)]
	public class RemoveTypeFromTimeEntryReport : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.TimeEntryReport", "Type");
		}
	}
}