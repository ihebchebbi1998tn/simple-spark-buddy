namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160623132700)]
	public class ChangeReplenishmentOrderToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ReplenishmentOrderItem_ReplenishmentOrder')
					BEGIN
						ALTER TABLE [SMS].[ReplenishmentOrderItem] DROP CONSTRAINT [FK_ReplenishmentOrderItem_ReplenishmentOrder]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ReplenishmentOrderItem')
					BEGIN
						ALTER TABLE [SMS].[ReplenishmentOrderItem] DROP CONSTRAINT [PK_ReplenishmentOrderItem]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ReplenishmentOrder')
					BEGIN
						ALTER TABLE [SMS].[ReplenishmentOrder] DROP CONSTRAINT [PK_ReplenishmentOrder]
					END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ReplenishmentOrder' AND COLUMN_NAME='ReplenishmentOrderId' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [SMS].[ReplenishmentOrder] ADD [ReplenishmentOrderIdOld] bigint NULL
					EXEC('UPDATE [SMS].[ReplenishmentOrder] SET [ReplenishmentOrderIdOld] = [ReplenishmentOrderId]')
					ALTER TABLE [SMS].[ReplenishmentOrder] DROP COLUMN [ReplenishmentOrderId]
					ALTER TABLE [SMS].[ReplenishmentOrder] ADD [ReplenishmentOrderId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [SMS].[ReplenishmentOrder] ADD  CONSTRAINT [PK_ReplenishmentOrder] PRIMARY KEY CLUSTERED ([ReplenishmentOrderId] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ReplenishmentOrderItem' AND COLUMN_NAME='ReplenishmentOrderItemId' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD [ReplenishmentOrderItemIdOld] bigint NULL
					EXEC('UPDATE [SMS].[ReplenishmentOrderItem] SET [ReplenishmentOrderItemIdOld] = [ReplenishmentOrderItemId]')
					ALTER TABLE [SMS].[ReplenishmentOrderItem] DROP COLUMN [ReplenishmentOrderItemId]
					ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD [ReplenishmentOrderItemId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD  CONSTRAINT [PK_ReplenishmentOrderItem] PRIMARY KEY CLUSTERED ([ReplenishmentOrderItemId] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ReplenishmentOrderItem' AND COLUMN_NAME='ReplenishmentOrderKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[SMS].[ReplenishmentOrderItem].[ReplenishmentOrderKey]', 'ReplenishmentOrderKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD [ReplenishmentOrderKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ReplenishmentOrderKey] = b.[ReplenishmentOrderId] FROM [SMS].[ReplenishmentOrderItem] a LEFT OUTER JOIN [SMS].[ReplenishmentOrder] b ON a.[ReplenishmentOrderKeyOld] = b.[ReplenishmentOrderIdOld]')
					ALTER TABLE [SMS].[ReplenishmentOrderItem] ALTER COLUMN [ReplenishmentOrderKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ReplenishmentOrderItem] ALTER COLUMN [ReplenishmentOrderKeyOld] bigint NULL
					ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD CONSTRAINT [FK_ReplenishmentOrderItem_ReplenishmentOrder] FOREIGN KEY([ReplenishmentOrderKey]) REFERENCES[SMS].[ReplenishmentOrder]([ReplenishmentOrderId]) ON UPDATE CASCADE ON DELETE CASCADE
				END");
		}
	}
}