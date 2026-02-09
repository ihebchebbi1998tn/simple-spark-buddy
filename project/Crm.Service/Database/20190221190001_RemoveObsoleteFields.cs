namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190221190001)]
	public class RemoveObsoleteFields : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserFlag01", 0);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserFlag02", 0);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserFlag03", 0);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserString01", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserString02", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserString03", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserString04", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserString05", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserString05", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserDate01", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserDate02", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPos", "UserDate03", null);

			Database.RemoveColumnIfEmpty("SMS.InstallationPosSerials", "Attribute01", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPosSerials", "Attribute02", null);
			Database.RemoveColumnIfEmpty("SMS.InstallationPosSerials", "Attribute03", null);

			Database.RemoveColumnIfEmpty("SMS.ServiceOrderMaterial", "Attribute01", null);
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderMaterial", "Attribute02", null);
			Database.RemoveColumnIfEmpty("SMS.ServiceOrderMaterial", "Attribute03", null);
		}
	}
}