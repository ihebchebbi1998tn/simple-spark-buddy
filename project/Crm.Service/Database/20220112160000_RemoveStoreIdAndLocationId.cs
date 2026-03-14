namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220112160000)]
	public class RemoveStoreIdAndLocationId : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ServiceOrderMaterial", "LocationId");
			Database.RemoveColumnIfExisting("SMS.ServiceOrderMaterial", "StoreId");
			
		}
	}
}
