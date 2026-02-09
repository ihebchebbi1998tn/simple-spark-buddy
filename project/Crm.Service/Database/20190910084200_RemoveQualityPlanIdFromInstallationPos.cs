namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190910084200)]
	public class RemoveQualityPlanIdFromInstallationPos : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.InstallationPos", "QualityPlanId");
		}
	}
}