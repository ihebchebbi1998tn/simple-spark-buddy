namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200422110000)]
	public class MaterialDiscountCurrencyToDecimal : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF EXISTS (SELECT * FROM sys.columns WHERE [Name] = 'DiscountCurrency' AND object_id = OBJECT_ID('SMS.ServiceOrderMaterial') AND system_type_id = TYPE_ID('REAL'))
				BEGIN
					ALTER TABLE SMS.ServiceOrderMaterial
					ALTER COLUMN [DiscountCurrency] DECIMAL(18,2) NULL
				END
			");
		}
	}
}