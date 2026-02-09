namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20221103115500)]
	public class FlattenServiceOrderMaterialSerial : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("SerialNo", DbType.String, 250, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("SerialId", DbType.Guid, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("PreviousSerialNo", DbType.String, 250, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("PreviousSerialId", DbType.Guid, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("NoPreviousSerialNoReasonKey", DbType.String, 250, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("IsInstalled", DbType.Int32, ColumnProperty.Null));

			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("TEMP_HasSerial", DbType.Boolean, ColumnProperty.NotNull, 0));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("TEMP_SourceId", DbType.Guid, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("TEMP_RowNumber", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("TEMP_IgnoreInLoop", DbType.Int32, ColumnProperty.Null));

			Database.ExecuteNonQuery("Update SMS.ServiceOrderMaterial SET TEMP_HasSerial = 1 Where Id in (Select OrderMaterialId FROM SMS.ServiceOrderMaterialSerials Where IsActive = 1) OR SMS.ServiceOrderMaterial.IsSerial = 1");
			Database.ExecuteNonQuery(
				@"Insert Into SMS.ServiceOrderMaterial(
                                     [PosNo],
                                     [ItemNo],
                                     [Description],
                                     [InternalRemark],
                                     [QuantityUnit],
                                     [ActualQuantity],
                                     [EstimatedQuantity],
                                     [HourlyRate],
                                     [TotalValue],
                                     [DiscountPercent],
                                     [FromWarehouse],
                                     [FromLocationNo],
                                     [ToWarehouse],
                                     [ToLocationNo],
                                     [Status],
                                     [TransferDate],
                                     [BuiltIn],
                                     [IsSerial],
                                     [CreatedLocal],
                                     [CreateDate],
                                     [ModifyDate],
                                     [ServiceOrderTimeId],
                                     [Id],
                                     [Favorite],
                                     [SortOrder],
                                     [CreateUser],
                                     [ModifyUser],
                                     [InvoiceQuantity],
                                     [IsExported],
                                     [SignedByCustomer],
                                     [ExternalRemark],
                                     [IsActive],
                                     [LegacyVersion],
                                     [DispatchIdOld],
                                     [DispatchId],
                                     [ReplenishmentOrderItemIdOld],
                                     [ReplenishmentOrderItemId],
                                     [CommissioningStatusKey],
                                     [OrderId],
                                     [ArticleId],
                                     [ArticleTypeKey],
                                     [SerialNo],
                                     [PreviousSerialNo],
                                     [NoPreviousSerialNoReasonKey],
                                     [IsInstalled], 
                                     DiscountType,
                                     TEMP_SourceId,
                                     TEMP_RowNumber)
			Select
	   				[material].[PosNo],
			    	[material].[ItemNo],
			        [material].[Description],
			        [material].[InternalRemark],
			        [material].[QuantityUnit],
			        1,
			        1,
			        [material].[HourlyRate],
			        [material].[TotalValue] / [material].[InvoiceQuantity],
			        [material].[DiscountPercent],
			        [material].[FromWarehouse],
			        [material].[FromLocationNo],
			        [material].[ToWarehouse],
			        [material].[ToLocationNo],
			        [material].[Status],
			        [material].[TransferDate],
			        [material].[BuiltIn],
			        [material].[IsSerial],
			        [material].[CreatedLocal],
			        GETUTCDATE(),
			        GETUTCDATE(),
			        [material].[ServiceOrderTimeId],
			        NEWID(),
			        [material].[Favorite],
			        [material].[SortOrder],
			        [material].[CreateUser],
			        [material].[ModifyUser],
			        1,
			        [material].[IsExported],
			        [material].[SignedByCustomer],
			        [material].[ExternalRemark],
			        [material].[IsActive],
			        [material].[LegacyVersion],
			        [material].[DispatchIdOld],
			        [material].[DispatchId],
			        [material].[ReplenishmentOrderItemIdOld],
			        [material].[ReplenishmentOrderItemId],
			        [material].[CommissioningStatusKey],
			        [material].[OrderId],
			        [material].[ArticleId],
			        [material].[ArticleTypeKey],
			        [serial].[SerialNo],
			        [serial].[PreviousSerialNo],
			        [serial].[NoPreviousSerialNoReason],
			        [serial].[IsInstalled],
			        [material].[DiscountType],
			       	[material].Id,
			       	row_number() over (PARTITION BY [material].Id ORDER BY [material].Id)
			From SMS.ServiceOrderMaterialSerials as [serial] JOIN SMS.ServiceOrderMaterial as [material] ON material.Id = serial.OrderMaterialId Where [serial].IsActive = 1 AND [material].IsActive = 1");


			Database.AddTable("SMS.TEMP_Numbers", new Column("number", DbType.Int64, ColumnProperty.NotNull));
			Database.ExecuteScalar(@"
DECLARE @i int = 1
Declare @q int
Select @q = max(qty) From
(Select max(EstimatedQuantity) as qty From SMS.ServiceOrderMaterial Where EstimatedQuantity > ActualQuantity AND IsSerial = 1 AND IsActive = 1
UNION ALL 
Select max(InvoiceQuantity) as qty From SMS.ServiceOrderMaterial Where InvoiceQuantity > ActualQuantity AND IsSerial = 1 AND IsActive = 1)
as qty
WHILE (@i <= @q) 
BEGIN
	Insert into SMS.TEMP_Numbers(number) VALUES(@i)
	SET @i = @i + 1
END");
			Database.ExecuteNonQuery(@"
			Insert Into SMS.ServiceOrderMaterial(
					[PosNo], [ItemNo], [Description], [InternalRemark], [QuantityUnit],
					[ActualQuantity], [EstimatedQuantity], [HourlyRate], [TotalValue],
					[DiscountPercent], [FromWarehouse], [FromLocationNo], [DiscountType],
					[ToWarehouse], [ToLocationNo], [Status], [TransferDate], [BuiltIn], [IsSerial],
					[CreatedLocal], [CreateDate], [ModifyDate], [ServiceOrderTimeId], [Id],
					[Favorite], [SortOrder], [CreateUser], [ModifyUser], [InvoiceQuantity],
					[IsExported], [SignedByCustomer], [ExternalRemark], [IsActive], [LegacyVersion],
					[DispatchIdOld], [DispatchId], [ReplenishmentOrderItemIdOld], [ReplenishmentOrderItemId],
					[CommissioningStatusKey], [OrderId], [ArticleId], [ArticleTypeKey], 
					[SerialNo],	[PreviousSerialNo], [NoPreviousSerialNoReasonKey], [IsInstalled], [TEMP_IgnoreInLoop])
			Select 
					[PosNo], [ItemNo], [Description], [InternalRemark], [QuantityUnit],
					0, 1, [HourlyRate], 0, 
					[DiscountPercent], [FromWarehouse], [FromLocationNo],  [DiscountType],
					[ToWarehouse], [ToLocationNo], [Status], [TransferDate], [BuiltIn], [IsSerial],
					[CreatedLocal], GETUTCDATE(), GETUTCDATE(), [ServiceOrderTimeId], NEWID(), 
					[Favorite], [SortOrder], [CreateUser], [ModifyUser], 0, 
					[IsExported], [SignedByCustomer], [ExternalRemark], [IsActive], [LegacyVersion],
					[DispatchIdOld], [DispatchId], [ReplenishmentOrderItemIdOld], [ReplenishmentOrderItemId],
					[CommissioningStatusKey], [OrderId], [ArticleId], [ArticleTypeKey], 
					[SerialNo],	[PreviousSerialNo], [NoPreviousSerialNoReasonKey], [IsInstalled], 1 
			From SMS.TEMP_Numbers n inner join SMS.ServiceOrderMaterial som
				ON som.EstimatedQuantity - som.ActualQuantity >= n.number
			WHERE EstimatedQuantity > ActualQuantity AND IsSerial = 1 AND TEMP_IgnoreInLoop IS Null AND IsActive = 1");

			Database.ExecuteNonQuery(@"
			Insert Into SMS.ServiceOrderMaterial(
					[PosNo], [ItemNo], [Description], [InternalRemark], [QuantityUnit],
					[ActualQuantity], [EstimatedQuantity], [HourlyRate], [TotalValue],
					[DiscountPercent], [FromWarehouse], [FromLocationNo], [DiscountType],
					[ToWarehouse], [ToLocationNo], [Status], [TransferDate], [BuiltIn], [IsSerial],
					[CreatedLocal], [CreateDate], [ModifyDate], [ServiceOrderTimeId], [Id],
					[Favorite], [SortOrder], [CreateUser], [ModifyUser], [InvoiceQuantity],
					[IsExported], [SignedByCustomer], [ExternalRemark], [IsActive], [LegacyVersion],
					[DispatchIdOld], [DispatchId], [ReplenishmentOrderItemIdOld], [ReplenishmentOrderItemId],
					[CommissioningStatusKey], [OrderId], [ArticleId], [ArticleTypeKey], 
					[SerialNo],	[PreviousSerialNo], [NoPreviousSerialNoReasonKey], [IsInstalled], [TEMP_IgnoreInLoop])
			Select 
					[PosNo], [ItemNo], [Description], [InternalRemark], [QuantityUnit],
					0, 0, [HourlyRate], [TotalValue] / [InvoiceQuantity], 
					[DiscountPercent], [FromWarehouse], [FromLocationNo],  [DiscountType],
					[ToWarehouse], [ToLocationNo], [Status], [TransferDate], [BuiltIn], [IsSerial],
					[CreatedLocal], GETUTCDATE(), GETUTCDATE(), [ServiceOrderTimeId], NEWID(), 
					[Favorite], [SortOrder], [CreateUser], [ModifyUser], 1, 
					[IsExported], [SignedByCustomer], [ExternalRemark], [IsActive], [LegacyVersion],
					[DispatchIdOld], [DispatchId], [ReplenishmentOrderItemIdOld], [ReplenishmentOrderItemId],
					[CommissioningStatusKey], [OrderId], [ArticleId], [ArticleTypeKey], 
					[SerialNo],	[PreviousSerialNo], [NoPreviousSerialNoReasonKey], [IsInstalled], 1 
			From SMS.TEMP_Numbers n inner join SMS.ServiceOrderMaterial som
				ON som.InvoiceQuantity - som.ActualQuantity >= n.number
			WHERE InvoiceQuantity > ActualQuantity AND IsSerial = 1 AND TEMP_IgnoreInLoop IS Null AND IsActive = 1");

			//this handles when the estimated or invoiced qtys are less or equal than the actual
			Database.ExecuteNonQuery("Update n Set EstimatedQuantity = 0 FROM SMS.ServiceOrderMaterial as n INNER JOIN SMS.ServiceOrderMaterial as o On n.TEMP_SourceId = o.Id WHERE n.TEMP_RowNumber > o.EstimatedQuantity");
			Database.ExecuteNonQuery("Update n Set InvoiceQuantity = 0 FROM SMS.ServiceOrderMaterial as n INNER JOIN SMS.ServiceOrderMaterial as o On n.TEMP_SourceId = o.Id WHERE n.TEMP_RowNumber > o.InvoiceQuantity");

			Database.ExecuteNonQuery("Update SMS.ServiceOrderMaterial Set IsActive = 0 Where TEMP_HasSerial = 1");

			Database.RemoveColumn("SMS.ServiceOrderMaterial", "TEMP_HasSerial");
			Database.RemoveColumn("SMS.ServiceOrderMaterial", "TEMP_SourceId");
			Database.RemoveColumn("SMS.ServiceOrderMaterial", "TEMP_RowNumber");
			Database.RemoveColumn("SMS.ServiceOrderMaterial", "TEMP_IgnoreInLoop");
			Database.RemoveTable("SMS.TEMP_Numbers");
		}
	}
}
