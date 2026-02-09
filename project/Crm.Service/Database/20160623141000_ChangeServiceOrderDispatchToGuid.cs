namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160623141000)]
	public class ChangeServiceOrderDispatchToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderMaterial_ServiceOrderDispatch')
					BEGIN
						ALTER TABLE [SMS].[ServiceOrderMaterial] DROP CONSTRAINT [FK_ServiceOrderMaterial_ServiceOrderDispatch]
					END");

			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderTimePostings_ServiceOrderDispatch')
					BEGIN
						ALTER TABLE [SMS].[ServiceOrderTimePostings] DROP CONSTRAINT [FK_ServiceOrderTimePostings_ServiceOrderDispatch]
					END");

			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_ServiceOrderDispatch')
					BEGIN
						ALTER TABLE [SMS].[ServiceOrderDispatch] DROP CONSTRAINT [PK_ServiceOrderDispatch]
					END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderDispatch' AND COLUMN_NAME='DispatchId' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderDispatch] ADD [DispatchIdOld] bigint NULL
					EXEC('UPDATE [SMS].[ServiceOrderDispatch] SET [DispatchIdOld] = [DispatchId]')
					ALTER TABLE [SMS].[ServiceOrderDispatch] DROP COLUMN [DispatchId]
					ALTER TABLE [SMS].[ServiceOrderDispatch] ADD [DispatchId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [SMS].[ServiceOrderDispatch] ADD CONSTRAINT [PK_ServiceOrderDispatch] PRIMARY KEY CLUSTERED ([DispatchId] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderMaterial' AND COLUMN_NAME='DispatchId' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderMaterial].[DispatchId]', 'DispatchIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderMaterial] ADD [DispatchId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DispatchId] = b.[DispatchId] FROM [SMS].[ServiceOrderMaterial] a LEFT OUTER JOIN [SMS].[ServiceOrderDispatch] b ON a.[DispatchIdOld] = b.[DispatchIdOld]')
					ALTER TABLE [SMS].[ServiceOrderMaterial] ALTER COLUMN [DispatchIdOld] bigint NULL
					ALTER TABLE [SMS].[ServiceOrderMaterial] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderMaterial_ServiceOrderDispatch] FOREIGN KEY([DispatchId]) REFERENCES[SMS].[ServiceOrderDispatch]([DispatchId])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderMaterial' AND COLUMN_NAME='ReplenishmentOrderItemId' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderMaterial].[ReplenishmentOrderItemId]', 'ReplenishmentOrderItemIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderMaterial] ADD [ReplenishmentOrderItemId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ReplenishmentOrderItemId] = b.[ReplenishmentOrderItemId] FROM [SMS].[ServiceOrderMaterial] a LEFT OUTER JOIN [SMS].[ReplenishmentOrderItem] b ON a.[ReplenishmentOrderItemIdOld] = b.[ReplenishmentOrderItemIdOld]')
					ALTER TABLE [SMS].[ServiceOrderMaterial] ALTER COLUMN [ReplenishmentOrderItemIdOld] bigint NULL
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderTimePostings' AND COLUMN_NAME='DispatchId' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[SMS].[ServiceOrderTimePostings].[DispatchId]', 'DispatchIdOld', 'COLUMN'
					ALTER TABLE [SMS].[ServiceOrderTimePostings] ADD [DispatchId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DispatchId] = b.[DispatchId] FROM [SMS].[ServiceOrderTimePostings] a LEFT OUTER JOIN [SMS].[ServiceOrderDispatch] b ON a.[DispatchIdOld] = b.[DispatchIdOld]')
					ALTER TABLE [SMS].[ServiceOrderTimePostings] ALTER COLUMN [DispatchIdOld] bigint NULL
					ALTER TABLE [SMS].[ServiceOrderTimePostings] ADD CONSTRAINT [FK_ServiceOrderTimePostings_ServiceOrderDispatch] FOREIGN KEY([DispatchId]) REFERENCES[SMS].[ServiceOrderDispatch]([DispatchId])	
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Note' AND COLUMN_NAME='DispatchId' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[Note].[DispatchId]', 'DispatchIdOld', 'COLUMN'
					ALTER TABLE [CRM].[Note] ADD [DispatchId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DispatchId] = b.[DispatchId] FROM [CRM].[Note] a LEFT OUTER JOIN [SMS].[ServiceOrderDispatch] b ON a.[DispatchIdOld] = b.[DispatchIdOld]')
					ALTER TABLE [CRM].[Note] ALTER COLUMN [DispatchIdOld] bigint NULL
				END");
		}
	}
}