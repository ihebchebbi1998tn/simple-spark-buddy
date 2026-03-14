namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210124145000)]
	public class AddSettableStatusToReadyForInvoiceStatus : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderStatus SET SettableStatuses=CONCAT('PostProcessing,', SettableStatuses) WHERE SettableStatuses not like '%PostProcessing%' AND Value='ReadyForInvoice'");
		}
	}
}