namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20200219132300)]
	public class MigrateTimeEntryReportToPerDiemReport : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[CRM].[PerDiemReport]") && Database.TableExists("[SMS].[TimeEntryReport]"))
			{
				Database.AddColumn("[SMS].[ServiceOrderTimePostings]", new Column("PerDiemReportId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceOrderTimePostings_PerDiemReport", "SMS.ServiceOrderTimePostings", "PerDiemReportId", "CRM.PerDiemReport", "PerDiemReportId");
				Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderTimePostings] SET [PerDiemReportId] = [TimeEntryReportId] WHERE [TimeEntryReportId] IS NOT NULL");
				Database.RemoveForeignKeyIfExisting("SMS", "ServiceOrderTimePostings", "FK_ServiceOrderTimePostings_TimeEntryReport");
				Database.RemoveColumn("[SMS].[ServiceOrderTimePostings]", "TimeEntryReportId");
			}
		}
	}
}