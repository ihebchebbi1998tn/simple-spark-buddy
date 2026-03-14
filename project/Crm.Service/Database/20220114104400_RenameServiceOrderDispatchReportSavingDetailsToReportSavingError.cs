namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220114104400)]
	public class RenameServiceOrderDispatchReportSavingDetailsToReportSavingError : Migration
	{
		public override void Up()
		{
			Database.RenameColumn("SMS.ServiceOrderDispatch", "ReportSavingDetails", "ReportSavingError");
		}
	}
}