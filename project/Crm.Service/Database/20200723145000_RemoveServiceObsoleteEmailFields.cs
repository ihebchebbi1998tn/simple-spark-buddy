namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20200723145000)]
	public class RemoveServiceObsoleteEmailFields : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderDispatch SET ReportSent = 1 WHERE ReportSendingRetries = 5");
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderDispatch", "ReportSendingRetries", null);
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderDispatch", "ReportSendingDetails", null);
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderDispatch", "ReportRecipientsSentInternal", null);
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderDispatch", "ReportRecipientsSentExternal", null);

			Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderHead SET ReportSent = 1 WHERE ReportSendingRetries = 5");
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderHead", "ReportSendingDetails", null);
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderHead", "ReportSendingRetries", null);
		}
	}
}
