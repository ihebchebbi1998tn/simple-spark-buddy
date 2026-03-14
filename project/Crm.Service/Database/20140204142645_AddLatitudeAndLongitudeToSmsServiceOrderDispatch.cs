namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140204142645)]
	public class AddLatitudeAndLongitudeToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("Latitude", DbType.Double, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("Longitude", DbType.Double, ColumnProperty.Null));
		}
		public override void Down()
		{
		}
	}
}