namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130827154610)]
	public class RemoveSmsRemoteControlType : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.InstallationHead", "RemoteControlType");
			if (Database.TableExists("SMS.RemoteControlType"))
			{
				Database.RemoveTable("SMS.RemoteControlType");
			}
		}
	}
}