using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20160907091900)]
	public class AddIsSentAndRetryCounterToReplenishmentOrder : Migration
	{
		public override void Up()
		{
			if(!Database.ColumnExists("[SMS].[ReplenishmentOrder]", "IsSent"))
			{
				Database.AddColumn("[SMS].[ReplenishmentOrder]", new Column("IsSent", System.Data.DbType.Boolean, ColumnProperty.NotNull, false));
				Database.ExecuteNonQuery("UPDATE [SMS].[ReplenishmentOrder] SET IsSent = 1 WHERE IsClosed = 1");
			}
			if (!Database.ColumnExists("[SMS].[ReplenishmentOrder]", "RetryCounter"))
			{
				Database.AddColumn("[SMS].[ReplenishmentOrder]", new Column("RetryCounter", System.Data.DbType.Int32, ColumnProperty.NotNull, 0));
			}
		}
	}
}
