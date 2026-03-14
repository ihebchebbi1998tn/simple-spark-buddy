namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180820124800)]
	public class RemoveDateFromTimeEntryReport : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.TimeEntryReport", "Date");
		}
	}
}