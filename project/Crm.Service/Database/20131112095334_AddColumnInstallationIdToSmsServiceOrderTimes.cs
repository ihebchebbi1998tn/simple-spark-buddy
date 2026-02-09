namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131112095334)]
	public class AddColumnInstallationIdToSmsServiceOrderTimes : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimes", new Column("InstallationId", DbType.Int32, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}