namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140224141100)]
	public class ChangeSmsReplenishmentOrderItemToHighLowIds : Migration
	{
		private const int Low = 32;

		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrderItem] DROP CONSTRAINT PK_ReplenishmentOrderItem");
			Database.RenameColumn("[SMS].[ReplenishmentOrderItem]", "ReplenishmentOrderItemId", "Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD ReplenishmentOrderItemId bigint NULL");
			Database.ExecuteNonQuery("UPDATE [SMS].[ReplenishmentOrderItem] SET ReplenishmentOrderItemId = Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrderItem] ALTER COLUMN ReplenishmentOrderItemId bigint NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrderItem] DROP COLUMN Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD CONSTRAINT PK_ReplenishmentOrderItem PRIMARY KEY(ReplenishmentOrderItemId)");

			Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[SMS].[ReplenishmentOrderItem]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(ReplenishmentOrderItemId), 0) / " + Low + ") + 1 from [SMS].[ReplenishmentOrderItem] where ReplenishmentOrderItemId is not null), '[SMS].[ReplenishmentOrderItem]') END");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}