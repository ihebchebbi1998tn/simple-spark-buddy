namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230602120000)]
	public class FixQuantityTypes : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@$"
			ALTER TABLE
			  SMS.InstallationPos
			ALTER COLUMN
			  Quantity
			    decimal(19,5) NOT NULL;
			");
			Database.ExecuteNonQuery(@$"
			ALTER TABLE
			  SMS.ServiceOrderMaterial
			ALTER COLUMN
			  InvoiceQuantity
			    decimal(19,5) NOT NULL;
			");
			Database.ExecuteNonQuery(@$"
			ALTER TABLE
			  SMS.ServiceOrderMaterial
			ALTER COLUMN
			  ActualQuantity
			    decimal(19,5) NOT NULL;
			");
			Database.ExecuteNonQuery(@$"
			ALTER TABLE
			  SMS.ServiceOrderMaterial
			ALTER COLUMN
			  EstimatedQuantity
			    decimal(19,5) NOT NULL;
			");
		}
	}
}
