namespace Crm.Service.Database;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

[Migration(20220921091000)]
public class MigrateCurrencyFromMaterialToSO :Migration{
	public override void Up()
	{
		Database.ExecuteNonQuery(@"Update SO
SET SO.CurrencyKey = (
    Select TOP(1) currency.CurrencyKey FROM (
		Select CurrencyKey FROM SMS.ServiceOrderMaterial WHERE OrderId = SO.ContactKey AND IsActive = 1 AND CurrencyKey IS NOT NULL 
		UNION ALL
		Select CurrencyKey FROM SMS.ServiceOrderTimes WHERE OrderId = SO.ContactKey AND IsActive = 1 AND CurrencyKey IS NOT NULL) currency
    ) 
FROM SMS.ServiceOrderHead SO
WHERE SO.CurrencyKey is null
");
	}
}