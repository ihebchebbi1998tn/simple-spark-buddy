namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130904110626)]
	public class AddManufactureDateToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.InstallationHead", new Column("ManufactureDate", DbType.DateTime, ColumnProperty.Null));
		}
	}
}