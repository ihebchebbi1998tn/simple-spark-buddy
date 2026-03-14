namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180411170000)]
	public class RemoveTenantAndTenantUser : Migration
	{
		public override void Up()
		{
			Database.DropTableIfExistingAndEmpty("CRM", "UserTenant");
			Database.DropTableIfExistingAndEmpty("CRM", "Tenant");
		}
	}
}
