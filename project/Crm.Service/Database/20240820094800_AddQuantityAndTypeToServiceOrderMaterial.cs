namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240820094800)]
	public class AddQuantityAndTypeToServiceOrderMaterial : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderMaterial", "ServiceOrderMaterialType"))
			{
				Database.AddColumn("SMS.ServiceOrderMaterial",
					new Column("Quantity",
						DbType.Decimal,
						2,
						ColumnProperty.Null));
				Database.AddColumn("SMS.ServiceOrderMaterial",
					new Column("ServiceOrderMaterialType",
						DbType.String,
						ColumnProperty.Null));

				Database.ExecuteNonQuery("SELECT * INTO #ServiceOrderMaterialUsed FROM SMS.ServiceOrderMaterial WHERE EstimatedQuantity > 0 AND (ActualQuantity > 0 OR DispatchId IS NOT NULL)");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderMaterialUsed SET AuthDataId = NULL");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderMaterialUsed SET ParentServiceOrderMaterialId = [id]");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderMaterialUsed SET Quantity = ActualQuantity, ServiceOrderMaterialType = 'Used', [id] = newid()");

				Database.ExecuteNonQuery("SELECT SMS.ServiceOrderMaterial.* INTO #ServiceOrderMaterialInvoice FROM SMS.ServiceOrderMaterial JOIN SMS.ServiceOrderHead ON OrderId = ContactKey WHERE (EstimatedQuantity > 0 OR ActualQuantity > 0) AND (InvoiceQuantity > 0 OR SMS.ServiceOrderHead.Status IN ('PostProcessing', 'ReadyForInvoice', 'Invoiced', 'Closed'))");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderMaterialInvoice SET AuthDataId = NULL");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderMaterialInvoice SET ParentServiceOrderMaterialId = [id]");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderMaterialInvoice SET Quantity = InvoiceQuantity, ServiceOrderMaterialType = 'Invoice', [id] = newid()");

				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderMaterial SET ServiceOrderMaterialType = 'Preplanned', Quantity = EstimatedQuantity WHERE EstimatedQuantity > 0");
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderMaterial SET ServiceOrderMaterialType = 'Used', Quantity = ActualQuantity WHERE EstimatedQuantity = 0 AND ActualQuantity > 0");
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderMaterial SET ServiceOrderMaterialType = 'Invoice', Quantity = InvoiceQuantity WHERE ServiceOrderMaterialType IS NULL");
				Database.ExecuteNonQuery("INSERT INTO SMS.ServiceOrderMaterial SELECT * FROM #ServiceOrderMaterialUsed");
				Database.ExecuteNonQuery("INSERT INTO SMS.ServiceOrderMaterial SELECT * FROM #ServiceOrderMaterialInvoice");

				Database.ChangeColumn("SMS.ServiceOrderMaterial",
					new Column("Quantity",
						DbType.Decimal,
						2,
						ColumnProperty.NotNull));
				Database.ChangeColumn("SMS.ServiceOrderMaterial",
					new Column("ServiceOrderMaterialType",
						DbType.String,
						ColumnProperty.NotNull));
				Database.RemoveColumn("SMS.ServiceOrderMaterial", "ActualQuantity");
				Database.RemoveColumn("SMS.ServiceOrderMaterial", "EstimatedQuantity");
				Database.RemoveColumn("SMS.ServiceOrderMaterial", "InvoiceQuantity");
			}
		}
	}
}
