namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20141030092812)]
	public class AlterServiceOrderMaterialQuantityColumsToDecimal : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("SMS.ServiceOrderMaterial", "EstimatedQuantity"))
			{
				Database.ExecuteNonQuery(@"EXEC DropDefault 'SMS', 'ServiceOrderMaterial', 'EstimatedQuantity'
																		ALTER TABLE SMS.ServiceOrderMaterial ALTER COLUMN EstimatedQuantity decimal(18,2) NOT NULL
																		ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT [DF_ServiceOrderMaterial_EstimatedQuantity]  DEFAULT 0 FOR EstimatedQuantity");
			}
			if (Database.ColumnExists("SMS.ServiceOrderMaterial", "ActualQuantity"))
			{
				Database.ExecuteNonQuery(@"EXEC DropDefault 'SMS', 'ServiceOrderMaterial', 'ActualQuantity'
																		ALTER TABLE SMS.ServiceOrderMaterial ALTER COLUMN ActualQuantity decimal(18,2) NOT NULL
																		ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT [DF_ServiceOrderMaterial_ActualQuantity]  DEFAULT 0 FOR ActualQuantity");
			}
			if (Database.ColumnExists("SMS.ServiceOrderMaterial", "InvoiceQuantity"))
			{
				Database.ExecuteNonQuery(@"EXEC DropDefault 'SMS', 'ServiceOrderMaterial', 'InvoiceQuantity'
																		ALTER TABLE SMS.ServiceOrderMaterial ALTER COLUMN InvoiceQuantity decimal(18,2) NOT NULL 
																		ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT [DF_ServiceOrderMaterial_InvoiceQuantity]  DEFAULT 0 FOR [InvoiceQuantity]");
			}

			if (Database.ColumnExists("SMS.ServiceOrderMaterial", "DiscountPercent"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE SMS.ServiceOrderMaterial ALTER COLUMN DiscountPercent decimal(18,2)");
			}
			if (Database.ColumnExists("SMS.ServiceOrderMaterial", "DicountCurrency"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE SMS.ServiceOrderMaterial ALTER COLUMN DicountCurrency decimal(18,2)");
			}
		}
		public override void Down()
		{
		}
	}
}