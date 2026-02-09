namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220110141600)]
	public class RemoveReportSavingRetriesFromServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ServiceOrderDispatch", "ReportSavingRetries");
		}
	}
}