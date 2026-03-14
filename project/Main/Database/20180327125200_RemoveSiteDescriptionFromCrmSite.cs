namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180327125200)]
	public class RemoveSiteDescriptionFromCrmSite : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[CRM].[Site]", "SiteDescription");
		}
	}
}
