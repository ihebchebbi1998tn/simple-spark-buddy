namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200422113000)]
	public class TimeDiscountToDecimal : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF EXISTS (SELECT * FROM sys.columns WHERE [Name] = 'DiscountCurrency' AND object_id = OBJECT_ID('SMS.ServiceOrderTimes') AND system_type_id = TYPE_ID('REAL'))
				BEGIN
					ALTER TABLE SMS.ServiceOrderTimes
					ALTER COLUMN [DiscountCurrency] DECIMAL(18,2) NULL
				END
				IF EXISTS (SELECT * FROM sys.columns WHERE [Name] = 'DiscountPercent' AND object_id = OBJECT_ID('SMS.ServiceOrderTimes') AND system_type_id = TYPE_ID('REAL'))
				BEGIN
					ALTER TABLE SMS.ServiceOrderTimes
					ALTER COLUMN [DiscountPercent] DECIMAL(18,2) NULL
				END
			");
		}
	}
}