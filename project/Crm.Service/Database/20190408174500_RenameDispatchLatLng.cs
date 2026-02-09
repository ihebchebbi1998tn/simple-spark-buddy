namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190408174500)]
	public class RenameDispatchLatLng : Migration
	{
		public override void Up()
		{
			Database.RenameColumn("SMS.ServiceOrderDispatch", "Latitude", "LatitudeOnDispatchStart");
			Database.RenameColumn("SMS.ServiceOrderDispatch", "Longitude", "LongitudeOnDispatchStart");
		}
	}
}