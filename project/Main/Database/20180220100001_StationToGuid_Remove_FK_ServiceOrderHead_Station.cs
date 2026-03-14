namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180220100001)]
	public class StationToGuid_Remove_FK_ServiceOrderHead_Station : Migration
	{
		public override void Up()
		{
			Database.RemoveForeignKeyIfExisting("SMS", "ServiceOrderHead", "FK_ServiceOrderHead_Station");
		}
	}
}
