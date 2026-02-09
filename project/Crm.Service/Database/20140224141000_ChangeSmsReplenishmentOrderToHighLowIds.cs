namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140224141000)]
	public class ChangeSmsReplenishmentOrderToHighLowIds : Migration
	{
		private const int Low = 32;

		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrder] DROP CONSTRAINT PK_ReplenishmentOrder");
			Database.RenameColumn("[SMS].[ReplenishmentOrder]", "ReplenishmentOrderId", "Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrder] ADD ReplenishmentOrderId bigint NULL");
			Database.ExecuteNonQuery("UPDATE [SMS].[ReplenishmentOrder] SET ReplenishmentOrderId = Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrder] ALTER COLUMN ReplenishmentOrderId bigint NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrder] DROP COLUMN Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrder] ADD CONSTRAINT PK_ReplenishmentOrder PRIMARY KEY(ReplenishmentOrderId)");

			Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[SMS].[ReplenishmentOrder]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(ReplenishmentOrderId), 0) / " + Low + ") + 1 from [SMS].[ReplenishmentOrder] where ReplenishmentOrderId is not null), '[SMS].[ReplenishmentOrder]') END");

			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrderItem] ALTER COLUMN ReplenishmentOrderKey bigint NOT NULL");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}