namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	
	[Migration(20150413161400)]
	public class AddReplenishmentOrderKeyFkToSmsReplenishmentOrderItem : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ReplenishmentOrderItem_ReplenishmentOrder'") == 0)
			{
				Database.ExecuteNonQuery("DELETE roi FROM [SMS].[ReplenishmentOrderItem] roi LEFT OUTER JOIN [SMS].[ReplenishmentOrder] ro on roi.[ReplenishmentOrderKey] = ro.[ReplenishmentOrderId] WHERE ro.[ReplenishmentOrderId] IS NULL");
				Database.AddForeignKey("FK_ReplenishmentOrderItem_ReplenishmentOrder", "[SMS].[ReplenishmentOrderItem]", "ReplenishmentOrderKey", "[SMS].[ReplenishmentOrder]", "ReplenishmentOrderId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}