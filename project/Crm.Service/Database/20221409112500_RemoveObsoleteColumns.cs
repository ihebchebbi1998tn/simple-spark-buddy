namespace Crm.Service.Database;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

[Migration(20221409112500)]
public class RemoveObsoleteColumns :Migration{
	public override void Up()
	{
		Database.RemoveColumnIfEmpty("SMS.ServiceOrderTimes", "DiscountCurrency", null);
		Database.RemoveColumnIfEmpty("SMS.ServiceOrderTimes", "CurrencyKey", null);
		Database.RemoveColumnIfEmpty("SMS.ServiceOrderMaterial", "DiscountCurrency", null);
		Database.RemoveColumnIfEmpty("SMS.ServiceOrderMaterial", "CurrencyKey", null);
		Database.RemoveColumnIfEmpty("SMS.ServiceOrderTimePosting", "DiscountPercent", null);
	}
}