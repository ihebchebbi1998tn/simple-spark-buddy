namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190917162000)]
	public class AddColorToSmsServiceCaseCategory : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceNotificationCategory", new Column("Color", DbType.String, 20, ColumnProperty.NotNull, "'#9E9E9E'"));
		}
	}
}
