namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20211118144500)]
	public class RemoveReplenishmentOrderRetryCounter : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE SMS.ReplenishmentOrder SET IsSent = 1 WHERE RetryCounter = 3");
			Database.RemoveColumnIfEmpty("SMS.ReplenishmentOrder", "RetryCounter", 0);
		}
	}
}
