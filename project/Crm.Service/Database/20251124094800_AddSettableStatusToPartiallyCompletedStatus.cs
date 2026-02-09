using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20251124094800)]
	public class AddSettableStatusToPartiallyCompletedStatus : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderStatus SET SettableStatuses='Completed' WHERE Value='PartiallyCompleted'");
		}
	}
}
