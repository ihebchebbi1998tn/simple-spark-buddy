namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190917162100)]
	public class AddColorToSmsServiceCaseStatus : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceNotificationStatus", new Column("Color", DbType.String, 20, ColumnProperty.NotNull, "'#9E9E9E'"));
		}
	}
}
