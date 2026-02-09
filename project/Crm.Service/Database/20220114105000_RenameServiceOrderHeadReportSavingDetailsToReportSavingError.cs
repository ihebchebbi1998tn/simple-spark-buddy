namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220114105000)]
	public class RenameServiceOrderHeadReportSavingDetailsToReportSavingError : Migration
	{
		public override void Up()
		{
			Database.RenameColumn("SMS.ServiceOrderHead", "ReportSavingDetails", "ReportSavingError");
		}
	}
}