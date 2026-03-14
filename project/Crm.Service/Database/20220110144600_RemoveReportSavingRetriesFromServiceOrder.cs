namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220110144600)]
	public class RemoveReportSavingRetriesFromServiceOrder : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ServiceOrderHead", "ReportSavingRetries");
		}
	}
}