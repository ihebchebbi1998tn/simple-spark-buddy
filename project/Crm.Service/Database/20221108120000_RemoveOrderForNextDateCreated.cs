namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20221108120000)]
	public class RemoveOrderForNextDateCreated : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.MaintenancePlan", "OrderForNextDateCreated");
		}
	}
}
