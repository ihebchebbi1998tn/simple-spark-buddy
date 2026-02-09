namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130827154609)]
	public class AddColumnsRoomAndExactPlaceToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.InstallationHead", new Column("Room", DbType.String, 256, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.InstallationHead", new Column("ExactPlace", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}