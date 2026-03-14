namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200916163000)]
	public class AddCurrencyKeyToServiceOrderMaterialAndTimes : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CurrencyKey nvarchar(20)");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimes] ADD CurrencyKey nvarchar(20)");
		}
	}
}